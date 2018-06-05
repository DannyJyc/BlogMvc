using System;
using System.Linq;
using System.Web.Mvc;
using Blog.DAL;
using Blog.Models;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        BlogContext context = new BlogContext();
        // GET: Home

        public ActionResult Many(string name)
        {
            return Content("hello " + name);
        }

        [HttpGet]
        public ActionResult Index()
        {
            var postList = from p in context.Postses
                           select p;
            // jycsb这里应该限制去除最多多少条，不然前端应该预留给你多大的空间呢？
            var tagList = from ta in context.Tags
                          select ta;
            // jycsb什么他妈玩意儿，这里直接用LINQ只选出6条不就结了吗
            var clickList = from c in context.Postses
                            orderby c.Click descending
                            select c;
            // jycsb什么他妈玩意儿，这里直接用LINQ只选出6条不就结了吗
            var bydatePosts = from b in context.Postses
                              orderby b.CreateDate descending
                              select b;


            ViewBag.tag = tagList.Select(p => p.Name).Distinct().Take(20).ToList();
            ViewBag.sellist = clickList.Take(6).ToList();
            ViewBag.newsposts = bydatePosts.Take(6).ToList();

            return View(postList.Take(10));
        }

        public ActionResult About()
        {
            var newsposts = from po in context.Postses
                            orderby po.CreateDate descending
                            select po;
            ViewBag.newsposts = newsposts.Take(6).ToList();
            return View();
        }

        public ActionResult Single(int? id)
        {
            if (id == 0 || id == null)
            {
                return Redirect("Index");
            }

            var update = context.Postses.SingleOrDefault(p => p.PostsId == id);
            if (update != null) update.Click = update.Click + 1;
            context.SaveChanges();
            var tagList = from t in context.Tags
                          select t;
            var clickList = from c in context.Postses
                            orderby c.Click descending
                            select c;
            var bydatePosts = from b in context.Postses
                              orderby b.CreateDate descending
                              select b;
            var reply = from r in context.Replies
                        where r.PostsId == id && r.SecondReply == 0
                        select r;

            ViewBag.tag = tagList.Select(p => p.Name).Distinct().Take(20).ToList();
            ViewBag.sellist = clickList.Take(6).ToList();
            ViewBag.newsposts = bydatePosts.Take(6).ToList();
            ViewBag.reply = reply.ToList();
            var postList = from p in context.Postses
                           where p.PostsId == id
                           select p;
            var singleResult = postList.SingleOrDefault();
            return View(singleResult);
        }

        public ActionResult Success()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            var newsposts = from po in context.Postses
                            orderby po.CreateDate descending
                            select po;
            ViewBag.newsposts = newsposts.Take(6).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Contact(Message n)
        {
            if (Session["Name"] == null)
            {
                return Redirect("/Users/Login");
            }
            var message = new Message
            {
                Name = n.Name,
                Email = n.Email,
                Messages = n.Messages
            };
            context.Messages.Add(message);
            context.SaveChanges();
            return Redirect("Success");
        }

        //public ActionResult Users(string t)
        //{
        //    var tagList = from ta in context.Tags
        //        select ta;
        //    var clickList = from c in context.Postses
        //        orderby c.Click descending
        //        select c;
        //    var bydatePosts = from b in context.Postses
        //        orderby b.CreateDate descending
        //        select b;
        //    var userlist = (from user in context.Users
        //        where user.UserName == t
        //        select user).SingleOrDefault();
        //    var postList = from p in context.Postses
        //        where p.UserId == userlist.UserId
        //        select p;
        //    ViewBag.tag = tagList.Select(p => p.Name).Distinct().Take(20).ToList();
        //    ViewBag.sellist = clickList.Take(6).ToList();
        //    ViewBag.newsposts = bydatePosts.Take(6).ToList();
        //    ViewBag.polist = postList.ToList();
        //    return View("Older");
        //}

        [HttpPost]
        public ActionResult Single(Reply n, int id)
        {
            if (Session["Name"] == null)
            {
                return Redirect("/Users/Login");
            }
            int groupid = Convert.ToInt16(Session["GroupId"]);
            if (!GetValue.PerTwo(groupid))
            {
                return Redirect("/Backstage/Ero");

            }

            var reply = new Reply
            {
                Name = n.Name,
                Email = n.Email,
                ReplyContent = n.ReplyContent,
                CreateDate = DateTime.Now,
                SecondReply = n.SecondReply == 0 ? 0 : n.SecondReply,
                PostsId = id
            };
            context.Replies.Add(reply);
            context.SaveChanges();
            return Redirect("/Home/Single/" + id);
        }

        public ActionResult Older(string t,string username)
        {
            var postList = from p in context.Postses
                           select p;
            // jycsb这里应该限制去除最多多少条，不然前端应该预留给你多大的空间呢？
            var tagList = from ta in context.Tags
                          select ta;
            // jycsb什么他妈玩意儿，这里直接用LINQ只选出6条不就结了吗
            var clickList = from c in context.Postses
                            orderby c.Click descending
                            select c;
            // jycsb什么他妈玩意儿，这里直接用LINQ只选出6条不就结了吗
            var bydatePosts = from b in context.Postses
                              orderby b.CreateDate descending
                              select b;
            if (t != null)
            {
                //var tagname = (from tag in context.Tags
                //    where tag.TagId == id
                //    select tag).SingleOrDefault();
                postList = from p in context.Postses
                           from ta in p.Tags
                           where ta.Name == t
                           select p;
            }
            else if(username!=null)
            {
                var userlist = (from user in context.Users
                                where user.UserName == username
                                select user).SingleOrDefault();
                postList = from p in context.Postses
                           where p.UserId == userlist.UserId
                           select p;
            }
            ViewBag.tag = tagList.Select(p => p.Name).Distinct().Take(20).ToList();
            ViewBag.sellist = clickList.Take(6).ToList();
            ViewBag.newsposts = bydatePosts.Take(6).ToList();
            ViewBag.polist = postList.ToList();

            return View();
        }


        [HttpPost]
        public ActionResult Older()
        {
            if (Session["Name"] == null)
            {
                return Redirect("/Users/Login");
            }
            int groupid = Convert.ToInt16(Session["GroupId"]);
            if (!GetValue.PerThree(groupid))
            {
                return Redirect("/Backstage/Ero");

            }
            string search = Request["Search"];
            var tagList = from ta in context.Tags
                          select ta;
            // jycsb什么他妈玩意儿，这里直接用LINQ只选出6条不就结了吗
            var clickList = from c in context.Postses
                            orderby c.Click descending
                            select c;
            // jycsb什么他妈玩意儿，这里直接用LINQ只选出6条不就结了吗
            var bydatePosts = from b in context.Postses
                              orderby b.CreateDate descending
                              select b;
            var polist = from po in context.Postses
                         where (po.Title.Contains(search)) || (po.Outline.Contains(search)) || (po.Content.Contains(search))
                         select po;

            ViewData["search"] = search;
            ViewBag.tag = tagList.Select(p => p.Name).Distinct().Take(20).ToList();
            ViewBag.sellist = clickList.Take(6).ToList();
            ViewBag.newsposts = bydatePosts.Take(6).ToList();
            ViewBag.polist = polist.ToList();
            return View();
        }

    }
}