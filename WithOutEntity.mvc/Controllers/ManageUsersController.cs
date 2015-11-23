using System;
using System.IO;
using System.Data;
using System.Net;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;

using democode.mvc.Models;
using CustomSecurity;

namespace democode.mvc.Controllers
{
    [RoutePrefix("manageusers")]
    [Route("{action=index}")]
    public class ManageUsersController : Controllers.BaseController
    {
        #region "Helpers"
        List<democode.mvc.Models.UserModels> modellist = new List<democode.mvc.Models.UserModels>();

        private democode.mvc.Models.UserModels convertToModel(DataRow dr)
        {
            democode.mvc.Models.UserModels x = new democode.mvc.Models.UserModels();
            try
            {
                if (dr["UID"] != System.DBNull.Value) { x.UID = new Guid(dr["UID"].ToString()); }
                if (dr["APPID"] != System.DBNull.Value) { x.AppID = new Guid(dr["APPID"].ToString()); }
                if (dr["UserName"] != System.DBNull.Value) { x.Username = (String)dr["UserName"]; }
                if (dr["UserIsAnonymous"] != System.DBNull.Value) { x.IsAnonymous = Convert.ToBoolean(dr["UserIsAnonymous"]); }
                if (dr["UserLastActivityDate"] != System.DBNull.Value) { x.LastActivityDate = (DateTime)dr["UserLastActivityDate"]; }
                //if (dr["UserTimestamp"] != System.DBNull.Value) { x.TimeStamp = (DateTime)dr["UserTimestamp"]; }


                x.Demographics = new UserDemographics(dr);
                //load membership information for the current application
                x.Membership = new MembershipUser(ConfigurationManager.ConnectionStrings["SystemDS"].ToString(), x.UID, x.AppID);
                //load role for user
                x.Role = Role.getUserRoleForApplication(ConfigurationManager.ConnectionStrings["SystemDS"].ToString(), x.UID, x.AppID);

                return x;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private democode.mvc.Models.UserModels convertToModel(CustomSecurity.User data)
        {
            democode.mvc.Models.UserModels x = new democode.mvc.Models.UserModels();
            try
            {
                x.UID = data.UID;
                x.AppID = data.APPID;
                x.Username = data.UserName;
                x.IsAnonymous = data.IsAnonymous;
                x.LastActivityDate = data.LastActivityDate;
                //x.TimeStamp = data._timestamp;

                x.Demographics = data.Demographics;
                x.Membership = data.Membership;
                x.Role = data.Role;

                return x;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private CustomSecurity.User convertFromModel(democode.mvc.Models.UserModels data)
        {
            CustomSecurity.User x = new CustomSecurity.User();
            try
            {
                x.UID = data.UID;
                x.APPID = data.AppID;
                x.UserName = data.Username;
                x.IsAnonymous = data.IsAnonymous;
                x.LastActivityDate = data.LastActivityDate;
                //x.TimeStamp = data._timestamp;

                x.Demographics = data.Demographics;
                x.Membership = data.Membership;
                x.Role = data.Role;

                return x;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        // eg: /manageusers
        public ActionResult Index(string searchterm)
        {
            //check if user is logged in and authorized
            if (CurrentUser == null || !CurrentUser.Role.IsSystem)
            {
                return Redirect("/Home");
            }

            modellist = new List<democode.mvc.Models.UserModels>();

            DataTable dt;
            if (string.IsNullOrEmpty(searchterm))
            {
                dt = CustomSecurity.User.dgUsers(ConfigurationManager.ConnectionStrings["SystemDS"].ToString());
            }
            else
            { 
                dt = CustomSecurity.User.dgUsers(ConfigurationManager.ConnectionStrings["SystemDS"].ToString(), "UserName", searchterm.Trim());
            }
           

            if (dt == null && dt.Rows.Count == 0) return View(modellist);

            int totalRows = (dt.Rows.Count < 100) ? dt.Rows.Count - 1 : 100; //correct for off by one error

            for (int i = 0; i <= totalRows; i++)
            //foreach (DataRow dr in dt.Rows)
            {
                DataRow dr = dt.Rows[i];
                var ev = convertToModel(dr);
                if (ev != null) modellist.Add(ev);
            }

            return View(modellist);
        }

        // eg: /manageusers/edit/{uid}
        [Route("user/edit/{uid:guid}")]
        public ActionResult Edit(Guid? uid)
        {
            //check if user is logged in and authorized
            if (CurrentUser == null || !CurrentUser.Role.IsSystem)
            {
                return RedirectToAction("", "Home");
            }

            if (!uid.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            CustomSecurity.User u = new CustomSecurity.User(ConfigurationManager.ConnectionStrings["SystemDS"].ToString(), new Guid(uid.ToString()));
            if (u == null)
            {
                HttpNotFound();
            }

            //create dropdown, this is accomplished by making the datatable IEnumerable
            var dt = CustomSecurity.Role.ToList(ConfigurationManager.ConnectionStrings["SystemDS"].ToString());
            ViewBag.Roles = new SelectList(dt, "RoleID", "Name", u.Role.RoleID.ToString());

            ViewBag.Lanaguages = new SelectList(Globalization.Language.ToList(ConfigurationManager.ConnectionStrings["SystemDS"].ToString())
                , "Code", "Name_EN", u.Demographics.Lanaguage);

            return View(convertToModel(u));
        }
        // eg: /manageusers/edit/{uid}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("user/edit")]
        public ActionResult Edit(democode.mvc.Models.UserModels u, FormCollection form)
        {
            //create dropdown, this is accomplished by making the datatable IEnumerable
            var dt = CustomSecurity.Role.ToList(ConfigurationManager.ConnectionStrings["SystemDS"].ToString());
            ViewBag.Roles = new SelectList(dt, "RoleID", "Name", u.Role.RoleID.ToString());

            ViewBag.Lanaguages = new SelectList(Globalization.Language.ToList(ConfigurationManager.ConnectionStrings["SystemDS"].ToString())
                , "Code", "Name_EN", u.Demographics.Lanaguage);

            //any modification to the form inline will result in a false statement
            if (ModelState.IsValid)
            {
                CustomSecurity.User user = new CustomSecurity.User(ConfigurationManager.ConnectionStrings["SystemDS"].ToString(), u.UID);

                if (user == null)
                {
                    ViewBag.Message = "User dose not exist";
                    return View();
                }

                //get selected values from dropdowns
                var roleid = form["Roles"];

                user.UserName = u.Username;

                if (u.Demographics != null)
                {
                    user.Demographics.FirstName = u.Demographics.FirstName.Trim();
                    user.Demographics.LastName = u.Demographics.LastName.Trim();
                    user.Demographics.DateOfBirth = u.Demographics.DateOfBirth;
                    user.Demographics.Gender = u.Demographics.Gender;
                    user.Demographics.Lanaguage = u.Demographics.Lanaguage;
                    user.Demographics.Country = u.Demographics.Country;
                    user.Demographics.PostalCode = u.Demographics.PostalCode;
                    user.Demographics.PhoneMobile = u.Demographics.PhoneMobile;
                }


                //EditUser.Role
                //if the selected role is not the same as the one currently assigned
                //then remove and add new role; otherwise do nothing
                if (roleid != user.Role.RoleID.ToString())
                {
                    if (CustomSecurity.Role.RemoveUserFromRole(ConfigurationManager.ConnectionStrings["SystemDS"].ToString(), user.UID,
                        user.Role.RoleID))
                    {
                        CustomSecurity.Role.AddUserToRole(ConfigurationManager.ConnectionStrings["SystemDS"].ToString(), user.UID,
                           new Guid(roleid));
                    }
                    else
                    {
                        ViewBag.Message = CustomSecurity.Role.GetLastError.Message;
                    }
                }

                if (u.Membership != null)
                {
                    //EditUser.Membership
                    user.Membership.Email = u.Membership.Email;
                    //only update users password if one was entered
                    if (!string.IsNullOrEmpty(u.Membership.Password))
                    {
                        string salt;
                        user.Membership.Password = CustomSecurity.PasswordHash.CreateHash(u.Membership.Password, out salt);
                        user.Membership.PasswordSalt = salt;
                        user.Membership.PasswordFormat = (Int16)CustomSecurity.PasswordFormat.PBKDF2;
                    }
                }

                user.Update(ConfigurationManager.ConnectionStrings["SystemDS"].ToString());

                ViewBag.Message = "user updated";

                return RedirectToAction("");
            }
            else
            {
                ViewBag.Message = "Invalid Post";
                return View(u);
            }
        }
    }
}