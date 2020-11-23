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
        public ActionResult Registration()
        {
            return View(0);
        }

        [HttpPost]
        // RegistrationProcess (POST) - Get and process registration
        //                              information.
        public ActionResult Registration(AccountType type)
        {
            // Get the form information.
            var email = Request.Form["email"];
            var passwd = Request.Form["passwd"];
            var passwd2 = Request.Form["passwd2"];
            System.Diagnostics.Debug.WriteLine("Email:" + email);
            System.Diagnostics.Debug.WriteLine("Passwd:" + passwd);

            // Attempt to convert the provided name to an email address
            // and return an error if this is not possible.
            try
            { var address = new MailAddress(email); }
            catch (FormatException)
            { return View(-1); }

            // If the given email is in the database, return an error.
            var db = new PickUpOrderDBEntities2();
            var matches =
                db.Accounts.Where(e => e.Email.Equals(email));
            if (matches.Count() > 0)
                return View(-2);

            // Check whether the password is correct
            // and return an error if it is not.
            if (!passwd.Equals(passwd2))
                return View(-3);

            // If this is all correct, add the account
            // and redirect to the appropriate page.
            // FIXME: Add redirection based on account type later.
            System.Diagnostics.Debug.WriteLine("Redirecting...");
            db.Accounts.Add(new Account(email, passwd, type));
            db.SaveChanges();
            return Redirect("/Menu/Menu");
        }
    }
}