// AccountManagementController - A controller that prepares all
//                               AccountManagement pages.

using PickUpOrder.Models;
using System;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;

namespace PickUpOrder.Controllers
{
    public class AccountManagementController : Controller
    {
        // Management (GET) - Display the root management page.
        [HttpGet]
        public ActionResult AccountManagement()
        {
            // If the user is not logged in, redirect to the login page.
            if (!Request.Cookies.AllKeys.Contains("UserID"))
                return RedirectToAction("Login", "Login");

            return View();
        }

        // Help (GET) - Display the help page.
        [HttpGet]
        public ActionResult Help()
        {
            // If the user is not logged in, redirect to the login page.
            if (!Request.Cookies.AllKeys.Contains("UserID"))
                return RedirectToAction("Login", "Login");

            return View();
        }

        // ChangeEmail (GET) - Display the email change page.
        [HttpGet]
        public ActionResult ChangeEmail()
        {
            // If the user is not logged in, redirect to the login page.
            if (!Request.Cookies.AllKeys.Contains("UserID"))
                return RedirectToAction("Login", "Login");

            return View(0);
        }

        // ProcessEmailChange (GET) - Save the user's new email if it's valid.
        [HttpPost]
        public ActionResult ProcessEmailChange()
        {
            // If the user is not logged in, redirect to the login page.
            if (!Request.Cookies.AllKeys.Contains("UserID"))
                return RedirectToAction("Login", "Login");

            // Get the new email.
            string newEmail = Request.Form["email"];

            // Attempt to convert the provided name to an email address
            // and return an error if this is not possible.
            try
            {
                var address = new MailAddress(newEmail);
            }
            catch (FormatException)
            {
                return View("ChangeEmail", -1);
            }

            // If the given email is in the database, return an error.
            int IDNo = Account.GetCookieID(Request.Cookies["UserID"].Value);
            var db = new PickUpOrderDBEntities2();
            IQueryable<Account> matches =
                db.Accounts.Where(e => e.Email.Equals(newEmail));
            if (matches.Count() > 0)
            {
                // If the match has the same user ID as the current user,
                // then the returned error is different.
                if (matches.First().UserID == IDNo)
                    return View("ChangeEmail", -3);
                else
                    return View("ChangeEmail", -2);
            }

            // Otherwise, set the new email.
            db.Accounts.Find(IDNo).Email = newEmail;
            db.SaveChanges();

            return View("AccountManagement");
        }

        // ChangePassword (GET) - Display the password change page.
        [HttpGet]
        public ActionResult ChangePassword()
        {
            // If the user is not logged in, redirect to the login page.
            if (!Request.Cookies.AllKeys.Contains("UserID"))
                return RedirectToAction("Login", "Login");

            return View(0);
        }

        // ProcessPasswordChange (GET) - Save the user's new password
        //                               if it's valid.
        [HttpPost]
        public ActionResult ProcessPasswordChange()
        {
            // If the user is not logged in, redirect to the login page.
            if (!Request.Cookies.AllKeys.Contains("UserID"))
                return RedirectToAction("Login", "Login");

            // Get the new email.
            string newPasswd = Request.Form["passwd"];
            string newPasswd2 = Request.Form["passwd2"];

            // If the passwords do not match, return an error.
            if (!newPasswd.Equals(newPasswd2))
                return View("ChangePassword", -1);

            // If the new password has the same hash, return an error.
            int IDNo = Account.GetCookieID(Request.Cookies["UserID"].Value);
            var db = new PickUpOrderDBEntities2();
            if(db.Accounts.Find(IDNo).CheckPassword(newPasswd))
                return View("ChangePassword", -2);

            // Otherwise, save the new password.
            // Since the hash function is hidden,
            // use the Account constructor to hash.
            Account temp = new Account("", newPasswd, AccountType.Customer);
            db.Accounts.Find(IDNo).PasswordHash = temp.PasswordHash;
            db.SaveChanges();

            return View("AccountManagement");
        }
    }
}