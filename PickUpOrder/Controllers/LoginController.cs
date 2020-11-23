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
            return View(new Account());
        }

        [HttpPost]
        // Login (POST) - Get and process login credentials.
        public ActionResult Login(Account provided)
        {
            // Get the email and password.
            var emailProvided = provided.Email;
            var passwdProvided = Request.Form["passwd"];

            System.Diagnostics.Debug.WriteLine("Email:" + emailProvided);
            System.Diagnostics.Debug.WriteLine("Passwd:" + passwdProvided);
            // Attempt to convert the provided name to an email address
            // and return an error if this is not possible.
            try
            { var address = new MailAddress(emailProvided); }
            catch (FormatException)
            { return View("InvalidEmail"); }

            // Attempt to find the email address in the database
            // and return an error if this is not possible.
            var db = new PickUpOrderDBEntities2();
            var matches =
                db.Accounts.Where(e => e.Email.Equals(emailProvided));            
            var match = matches.FirstOrDefault();
            if (match == null)
                return View("UnknownEmail");

            // Check whether the password is correct
            // and return an error if it is not.
            if (!match.CheckPassword(passwdProvided))
                return View("IncorrectPassword");

            // If this is all correct, redirect to the appropriate page.
            // FIXME: Add redirection based on account type later.
            System.Diagnostics.Debug.WriteLine("Redirecting...");
            var targetController = new MenuController();
            return Redirect("/Menu/Menu");
        }
    }
}