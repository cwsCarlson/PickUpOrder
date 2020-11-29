// LoginController - A controller that prepares all Login pages.

using PickUpOrder.Models;
using System;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;

namespace PickUpOrder.Controllers
{
    public class LoginController : Controller
    {
        // Login (GET) - Display the standard login.
        [HttpGet]
        public ActionResult Login()
        {
            // If the user is logged in, redirect to the appropriate page.
            if (Request.Cookies.AllKeys.Contains("UserID"))
            {
                AccountType type =
                    Account.GetCookieType(Request.Cookies["UserID"].Value);
                switch (type)
                {
                    case AccountType.Employee:
                        return Redirect("/OrderList/OrderList");
                    case AccountType.Manager:
                        return Redirect("/MenuEditor/MenuEditor");
                    default:
                        return Redirect("/Menu/Menu");
                }
            }

            return View(0);
        }

        // Logout (GET) - Remove the user cookie.
        [HttpGet]
        public ActionResult Logout()
        {
            // Set the expiration date to the previous day
            // so it is automatically deleted upon retrieval.
            if (Request.Cookies.AllKeys.Contains("UserID"))
                Response.Cookies["UserID"].Expires = DateTime.Now.AddDays(-1);
            return View("Login", 0);
        }

        // ResetPassword (GET) - Render the ResetPassword page.
        [HttpGet]
        public ActionResult ResetPassword()
        {
            // If the user is logged in, redirect to the appropriate page.
            if (Request.Cookies.AllKeys.Contains("UserID"))
            {
                AccountType type =
                    Account.GetCookieType(Request.Cookies["UserID"].Value);
                switch (type)
                {
                    case AccountType.Employee:
                        return Redirect("/OrderList/OrderList");
                    case AccountType.Manager:
                        return Redirect("/MenuEditor/MenuEditor");
                    default:
                        return Redirect("/Menu/Menu");
                }
            }

            return View(0);
        }

        // Help (GET) - Render the help page.
        [HttpGet]
        public ActionResult Help()
        {
            // If the user is logged in, redirect to the appropriate page.
            if (Request.Cookies.AllKeys.Contains("UserID"))
            {
                AccountType type =
                    Account.GetCookieType(Request.Cookies["UserID"].Value);
                switch (type)
                {
                    case AccountType.Employee:
                        return Redirect("/OrderList/OrderList");
                    case AccountType.Manager:
                        return Redirect("/MenuEditor/MenuEditor");
                    default:
                        return Redirect("/Menu/Menu");
                }
            }

            // Display the view.
            return View();
        }

        // ProcessLogin (POST) - Get and process login credentials.
        [HttpPost]
        public ActionResult ProcessLogin()
        {
            // If the user is logged in, redirect to the appropriate page.
            if (Request.Cookies.AllKeys.Contains("UserID"))
            {
                AccountType type =
                    Account.GetCookieType(Request.Cookies["UserID"].Value);
                switch (type)
                {
                    case AccountType.Employee:
                        return Redirect("/OrderList/OrderList");
                    case AccountType.Manager:
                        return Redirect("/MenuEditor/MenuEditor");
                    default:
                        return Redirect("/Menu/Menu");
                }
            }

            // Get the email and password.
            string email = Request.Form["email"];
            string passwd = Request.Form["passwd"];

            // Attempt to convert the provided name to an email address
            // and return an error if this is not possible.
            try
            {
                var address = new MailAddress(email);
            }
            catch (FormatException)
            {
                return View("Login", -1);
            }

            // Attempt to find the email address in the database
            // and return an error if this is not possible.
            var db = new PickUpOrderDBEntities2();
            IQueryable<Account> matches =
                db.Accounts.Where(e => e.Email.Equals(email));
            Account match = matches.FirstOrDefault();
            if (match == null)
                return View("Login", -2);      

            // Check whether the password is correct
            // and return an error if it is not.
            if (!match.CheckPassword(passwd))
                return View("Login", -3);

            // Define a cookie for this user that expires in an hour.
            // Cookie format: All but last two bits are user ID.
            // The last two bits of the cookie indicate the user type
            // and will be checked against the database whenever appropriate.
            // xorVal was randomly selected and exists to hide the exact value.
            int cookieVal = ((match.UserID * 4) + match.Type) ^
                Properties.Settings.Default.xorVal;
            Response.Cookies["UserID"].Value = cookieVal.ToString();
            Response.Cookies["UserID"].Expires = DateTime.Now.AddHours(1);
            Response.Cookies["UserID"].Secure = true;

            // Redirect to the appropriate page.
            switch ((AccountType) match.Type)
            {
                case AccountType.Employee:
                    return Redirect("/OrderList/OrderList");
                case AccountType.Manager:
                    return Redirect("/MenuEditor/MenuEditor");
                default:
                    return Redirect("/Menu/Menu");
            }
        }

        // Reset (POST) - Get and process a password reset request.
        [HttpPost]
        public ActionResult Reset()
        {
            // If the user is logged in, redirect to the appropriate page.
            if (Request.Cookies.AllKeys.Contains("UserID"))
            {
                AccountType type =
                    Account.GetCookieType(Request.Cookies["UserID"].Value);
                switch (type)
                {
                    case AccountType.Employee:
                        return Redirect("/OrderList/OrderList");
                    case AccountType.Manager:
                        return Redirect("/MenuEditor/MenuEditor");
                    default:
                        return Redirect("/Menu/Menu");
                }
            }

            // Get the form information.
            string email = Request.Form["email"];
            string passwd = Request.Form["passwd"];
            string passwd2 = Request.Form["passwd2"];

            // Attempt to convert the provided name to an email address
            // and return an error if this is not possible.
            try
            {
                var address = new MailAddress(email);
            }
            catch (FormatException)
            {
                return View("ResetPassword", -1);
            }

            // Attempt to find the email address in the database
            // and return an error if this is not possible.
            var db = new PickUpOrderDBEntities2();
            IQueryable<Account> matches =
                db.Accounts.Where(e => e.Email.Equals(email));
            Account match = matches.FirstOrDefault();
            if (match == null)
                return View("Login", -2);

            // Check whether the passwords match
            // and return an error if they do not.
            if (!passwd.Equals(passwd2))
                return View("Login", -3);

            // Use an Account constructor to calculate the new password's hash.
            // AccountType.Customer is simply there so the constructor works.
            var newAccount = new Account(email, passwd, AccountType.Customer);
            db.Accounts.Find(match.UserID).PasswordHash =
                newAccount.PasswordHash;
            db.SaveChanges();

            // Redirect to the login page.
            return View("Login", 1);
        }
    }
}