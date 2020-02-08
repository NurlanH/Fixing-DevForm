﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using DevFormAz.DevFormData;
using DevFormAz.Helper;
using DevFormAz.Models;

namespace DevFormAz.Areas.AdminPanel.Controllers
{
    //[RouteArea("AdminPanel")]
    public class AdminHomeController : Controller
    {
        DevFormAzDataBase db;
        public AdminHomeController()
        {
            db = new DevFormAzDataBase();
        }

        [AdminAccess]
        public ActionResult Index()
        {
            AdminAccessControl vm = new AdminAccessControl()
            {
                Forms = db.Forms.ToList(),
                TagLists = db.TagLists.ToList(),
                Users = db.Users.ToList()
            };
            return View("Index",vm);
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (true)
            {
                var checkManager = db.Users.Where(e => e.Email == email).SingleOrDefault();

                if (checkManager != null && checkManager.UserDetail.Role == "admin")
                {
                    if (Crypto.VerifyHashedPassword(checkManager.Password, password))
                    {
                        Session["AdminId"] = checkManager.Id;
                        Session["UserId"] = checkManager.Id;
                        return RedirectToAction("Index", "AdminHome", new { area = "AdminPanel" });
                    }
                }

            }
            return RedirectToAction("Login", "AdminHome", new { area = "AdminPanel" });
        }


        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("Login", "AdminHome",new { area="AdminPanel"});

        }
    }
}