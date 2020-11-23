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
        [HttpGet]
        // Login (GET) - Display the standard login.
        public ActionResult Login()
        {
            return View(0);
        }

        [HttpGet]
        // Logout (GET) - Remove the user cookie.
        public ActionResult Logout()
        {
            // Set the expiration date to the previous day
            // so it is automatically deleted upon retrieval.
            if (Request.Cookies.AllKeys.Contains("UserID"))
                Response.Cookies["UserID"].Expires = DateTime.Now.AddDays(-1);
            return View("Login", 0);
        }

        [HttpPost]
        // ProcessLogin (POST) - Get and process login credentials.
        public ActionResult ProcessLogin()
        {
            // Get the email and password.
            var email = Request.Form["email"];
            var passwd = Request.Form["passwd"];

            // Attempt to convert the provided name to an email address
            // and return an error if this is not possible.
            try
            { var address = new MailAddress(email); }
            catch (FormatException)
            { return View("Login", -1); }

            // Attempt to find the email address in the database
            // and return an error if this is not possible.
            var db = new PickUpOrderDBEntities2();
            var matches =
                db.Accounts.Where(e => e.Email.Equals(email));
            var match = matches.FirstOrDefault();
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
    }
}