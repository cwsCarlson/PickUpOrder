// RegistrationController - A controller that prepares all Registration pages.

using PickUpOrder.Models;
using System;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;

namespace PickUpOrder.Controllers
{
    public class RegistrationController : Controller
    {
        // Registration (GET) - Display the standard registration page.
        [HttpGet]
        public ActionResult Registration()
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

        // SecretRegistration (GET) - Display the employee registration page.
        [HttpGet]
        public ActionResult SecretRegistration()
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

        // SecretRegistration (GET) - Display the employee registration page.
        [HttpGet]
        public ActionResult SuperSecretRegistration()
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

        // Registration (POST) - Get and process registration
        //                       information.
        [HttpPost]
        public ActionResult Registration(AccountType type)
        {
            // If the user is logged in, redirect to the appropriate page.
            if (Request.Cookies.AllKeys.Contains("UserID"))
            {
                type =
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
            var email = Request.Form["email"];
            var passwd = Request.Form["passwd"];
            var passwd2 = Request.Form["passwd2"];

            // Attempt to convert the provided name to an email address
            // and return an error if this is not possible.
            try
            {
                var address = new MailAddress(email);
            }
            catch (FormatException)
            {
                switch (type)
                {
                    case AccountType.Employee:
                        return View("SecretRegistration", -1);
                    case AccountType.Manager:
                        return View("SuperSecretRegistration", -1);
                    default:
                        return View(-1);
                }
            }

            // If the given email is in the database, return an error.
            var db = new PickUpOrderDBEntities2();
            IQueryable<Account> matches =
                db.Accounts.Where(e => e.Email.Equals(email));
            if (matches.Count() > 0)
            {
                switch (type)
                {
                    case AccountType.Employee:
                        return View("SecretRegistration", -2);
                    case AccountType.Manager:
                        return View("SuperSecretRegistration", -2);
                    default:
                        return View(-2);
                }
            }

            // Check whether the password is correct
            // and return an error if it is not.
            if (!passwd.Equals(passwd2))
            {
                switch (type)
                {
                    case AccountType.Employee:
                        return View("SecretRegistration", -3);
                    case AccountType.Manager:
                        return View("SuperSecretRegistration", -3);
                    default:
                        return View(-3);
                }
            }

            // Add the account.
            var newAccount = new Account(email, passwd, type);
            db.Accounts.Add(newAccount);
            db.SaveChanges();

            // Define a cookie for this user that expires in an hour.
            // Cookie format: All but last two bits are user ID.
            // The last two bits of the cookie indicate the user type
            // and will be checked against the database whenever appropriate.
            // xorVal was randomly selected and exists to hide the exact value.
            int cookieVal = ((newAccount.UserID * 4) + newAccount.Type) ^
                Properties.Settings.Default.xorVal;
            Response.Cookies["UserID"].Value = cookieVal.ToString();
            Response.Cookies["UserID"].Expires = DateTime.Now.AddHours(1);
            Response.Cookies["UserID"].Secure = true;

            // Redirect to the appropriate page.
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
    }
}