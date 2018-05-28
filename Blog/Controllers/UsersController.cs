using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
        public ActionResult Register(User n)
        {
            var users = new User();
            users.Name = n.Name;
            users.Email = n.Email;
            users.UserName = n.UserName;
            users.PassWord = n.PassWord;
            context.Users.Add(users);
            context.SaveChanges();
            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User n)
        {
            var UserList = (from u in context.Users
                where u.UserName == n.UserName && u.PassWord == n.PassWord
                select u).SingleOrDefault();
            if (UserList == null)
            {
                return View();
            }

            Session["Name"] = UserList.Name;
            Session["Email"] = UserList.Email;
            Session["UserId"] = UserList.UserId;
            return Redirect("/Home/Index");
        }

        public ActionResult Logout()
        {
            Session["Name"] = null;
            Session["Email"] = null;
            Session["UserId"] = null;

            return Redirect("/Home/Index");
        }
    }
}