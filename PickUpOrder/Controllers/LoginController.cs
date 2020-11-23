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

        [HttpPost]
        // ProcessLogin (POST) - Get and process login credentials.
        public ActionResult ProcessLogin()
        {
            // Get the email and password.
            var email = Request.Form["email"];
            var passwd = Request.Form["passwd"];

            System.Diagnostics.Debug.WriteLine("Email:" + email);
            System.Diagnostics.Debug.WriteLine("Passwd:" + passwd);
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

            // If this is all correct, redirect to the appropriate page.
            // FIXME: Add redirection based on account type later.
            System.Diagnostics.Debug.WriteLine("Redirecting...");
            var targetController = new MenuController();
            return Redirect("/Menu/Menu");
        }
    }
}