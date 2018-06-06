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
        public ActionResult Register(string Email, string UserName, string Pwd)
        {
            if (GetValue.WithoutRegisterName(UserName))
            {
                return Content("ero");
            }
            
            var users = new User();
            users.Email = Email;
            users.UserName = UserName;
            users.PassWord = GetValue.GetStrMd5(Pwd);
            users.GroupId = 2;
            //users.Power = 0;
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

            //Session["Power"] = UserList.Power;
            Session["Name"] = UserList.UserName;
            Session["Email"] = UserList.Email;
            Session["UserId"] = UserList.UserId;
            Session["GroupId"] = UserList.GroupId;
            return Content("suc");
        }

        public ActionResult Logout()
        {
            Session["Name"] = null;
            Session["Email"] = null;
            Session["UserId"] = null;
            Session["GroupId"] = null;


            return Redirect("/Home/Index");
        }

        public ActionResult List()
        {
            var userlist = from user in context.Users
                select user;
            var group = from gro in context.Groups
                select gro;
            ViewBag.Group = group.ToList();
            return View(userlist);
        }

        [HttpPost]
        public ActionResult List(string UserId, string GroupId)
        {
            string[] user = UserId.Split(',');
            string[] group = GroupId.Split(',');
            for (int i = 0; i < user.Length; i++)
            {
                int intuser = Convert.ToInt16(user[i]);
                var usersinglelist = context.Users.SingleOrDefault(u => u.UserId == intuser);
                usersinglelist.GroupId = Convert.ToInt16(group[i]);
                context.SaveChanges();
            }

            //var user = context.Users.SingleOrDefault(u => u.UserId == userId);
            //user.GroupId = Id;
            //context.SaveChanges();
            return Content("suc");
        }

        public ActionResult GroupAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GroupAdd(Group n)
        {
            var group = new Group();
            group.GroupName = n.GroupName;
            context.Groups.Add(group);
            context.SaveChanges();
            return Redirect("/Users/GroupList");
        }

        public ActionResult GroupList()
        {
            var group = from gro in context.Groups
                select gro;
            return View(group);
        }

        [HttpPost]
        public ActionResult GroupList(int GroupId, string Power)
        {
            var del = context.Permissions.Where(p => p.GroupId == GroupId).ToList();
            foreach (var groupdel in del)
            {
                context.Permissions.Remove(groupdel);
            }
            context.SaveChanges();

            var permission = new Permission();
            string[] powerarr = Power.Split(',');
            foreach (var item in powerarr)
            {
                if (item == "")
                {
                    continue;
                }

                permission.GroupId = GroupId;
                permission.PermissionId = Convert.ToInt16(item);
                context.Permissions.Add(permission);
                context.SaveChanges();
            }
            return Content("suc");
        }
    }
}