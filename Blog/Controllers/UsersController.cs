using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.DAL;
using Blog.Models;

namespace Blog.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        BlogContext context = new BlogContext();

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string Name, string Email, string UserName, string Pwd)
        {
            if (GetValue.WithoutRegisterName(Name))
            {
                return Content("ero");
            }
            
            var users = new User();
            users.Name = Name;
            users.Email = Email;
            users.UserName = UserName;
            users.PassWord = GetValue.GetStrMd5(Pwd);
            users.Power = 0;
            context.Users.Add(users);
            context.SaveChanges();
            return Content("suc");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Name, string Pwd)
        {

            string p = GetValue.GetStrMd5(Pwd);
            var UserList = (from u in context.Users
                            where u.UserName == Name && u.PassWord == p
                            select u).SingleOrDefault();
            if (UserList == null)
            {
                return Content("ero");
            }

            Session["Power"] = UserList.Power;
            Session["Name"] = UserList.Name;
            Session["Email"] = UserList.Email;
            Session["UserId"] = UserList.UserId;
            return Content("suc");
        }

        public ActionResult Logout()
        {
            Session["Name"] = null;
            Session["Email"] = null;
            Session["UserId"] = null;
            Session["Power"] = null;


            return Redirect("/Home/Index");
        }
    }
}