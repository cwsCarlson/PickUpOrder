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
        // Hash (POST) - Display the password hash.
        //               FIXME: This will be removed later.
        public ActionResult Login(Account provided)
        {
            System.Diagnostics.Debug.WriteLine("Email:" + provided.Email);
            System.Diagnostics.Debug.WriteLine("Passwd:" + provided.PasswordHash);
            // Attempt to convert the provided name to an email address
            // and return an error if this is not possible.
            try
            { var address = new MailAddress(provided.Email); }
            catch (FormatException)
            { return View(new Account()); }

            // Attempt to find the email address in the database
            // and return an error if this is not possible.
            var db = new PickUpOrderDBEntities2();
            Account match;
            var matches =
                db.Accounts.Where(e => e.Email.Equals(provided.Email));            
            match = matches.FirstOrDefault();
            if(match == null)
                return View(new Account());

            // Check whether the password is correct
            // and return an error if it is not.
            if (!match.CheckPassword(provided.PasswordHash))
                return View(new Account());

            // If this is all correct, redirect to the appropriate page.
            // FIXME: Add redirection based on account type later.
            System.Diagnostics.Debug.WriteLine("Redirecting...");
            var targetController = new MenuController();
            return Redirect("/Menu/Menu");
        }
    }
}