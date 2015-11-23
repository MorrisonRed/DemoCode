using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using democode.mvc.Models;
using System.Configuration;

namespace democode.mvc.Controllers
{
    public class LoginLogOutController : Controllers.BaseController
    {
        // GET: LoginLogOut
        public ActionResult Index()
        {
            if (CurrentUser == null)
            {
                return RedirectToAction("Login", "LoginLogout");

            }
            return View();
        }

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        // GET: Login
        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //return user's id and password hash as pipe delimited string uid|pswd
            string validpswdhash = CustomSecurity.User.getUIDAndPasswordHash(ConfigurationManager.ConnectionStrings["SystemDS"].ToString(),
                new Guid(ConfigurationManager.AppSettings["appid"].ToString()), model.Email);

            string[] uidandpswd = { "", "" };
            if (validpswdhash == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            uidandpswd = validpswdhash.Split('|');

            //if the value is empty then the user name does not exist in the system
            if (!string.IsNullOrEmpty(validpswdhash))
            {
                //confirm that he password is correct
                if (CustomSecurity.PasswordHash.ValidatePassword(model.Password, uidandpswd[1]))
                {
                    //valid user name and password -- log in user
                    //first we must get the uid from the system for them load the user will all there
                    //memberships
                    CurrentUser = new CustomSecurity.User(ConfigurationManager.ConnectionStrings["SystemDS"].ToString(), new Guid(uidandpswd[0]));

                    //username and password where used to login 
                    //so the user is authenticated for making changes to account
                    CurrentUser.IsAuthenticiated = true;

                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
                }

            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }
        }

        // GET: LoginLogOut
        public ActionResult LogOut()
        {
            Session.Abandon();
            //return View();
            return RedirectToAction("Index", "Home");
        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}