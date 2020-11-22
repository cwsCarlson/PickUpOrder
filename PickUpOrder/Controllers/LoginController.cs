// LoginController - A controller that prepares all Login pages.
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace PickUpOrder.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        // Login (GET) - Display the standard login.
        public ActionResult Login()
        {
            return View((object) null);
        }

        [HttpPost]
        // Hash (POST) - Display the password hash.
        //               FIXME: This will be removed later.
        public ActionResult Hash()
        {
            var shaInstance = SHA256.Create();
            var passwd = Request.Form["newpasswd"];
            var encoded = Encoding.UTF8.GetBytes(passwd);
            var hashed = shaInstance.ComputeHash(encoded);
            var hashedStr = Convert.ToBase64String(hashed);
            return View("Login", (object) hashedStr);
        }
    }
}