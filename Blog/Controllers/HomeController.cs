using System;
using System.Linq;
using System.Web.Mvc;
using Blog.Models;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        BlogContext context = new BlogContext();
        // GET: Home
        [HttpGet]
        public ActionResult Index(int? id)
        {
            var postList = from p in context.Postses
                           select p;
            // jycsb这里应该限制去除最多多少条，不然前端应该预留给你多大的空间呢？
            var tagList = from t in context.Tags
                select t;
            // jycsb什么他妈玩意儿，这里直接用LINQ只选出6条不就结了吗
            var clickList = from c in context.Postses
                            orderby c.Click descending
                            select c;
            // jycsb什么他妈玩意儿，这里直接用LINQ只选出6条不就结了吗
            var bydatePosts = from b in context.Postses
                              orderby b.CreateDate descending
                              select b;
            if (id != null)
            {
                var tagname = (from tag in context.Tags
                    where tag.TagId == id
                    select tag).SingleOrDefault();
                postList = from p in context.Postses
                           from ta in p.Tags
                           where ta.Name == tagname.Name
                           select p;
            }

            ViewBag.tag = tagList.Select(t=>t.Name).Distinct().Take(20).ToList();
            ViewBag.sellist = clickList.Take(6).ToList();
            ViewBag.newsposts = bydatePosts.Take(6).ToList();

            return View(postList);
        }

        [HttpPost]
        public ActionResult Index()
        {
            string search = Request["Search"];
            var list = from tag in context.Tags
                       select tag;
            var sellist = from po in context.Postses
                          orderby po.Click descending
                          select po;
            var newsposts = from po in context.Postses
                            orderby po.CreateDate descending
                            select po;
            var polist = from po in context.Postses
                         where (po.Title.Contains(search)) || (po.Outline.Contains(search)) || (po.Content.Contains(search))
                         select po;

            ViewData["search"] = search;
            ViewBag.tag = list.ToList();
            ViewBag.sellist = sellist.ToList();
            ViewBag.newsposts = newsposts.ToList();
            return View(polist);
        }

        public ActionResult About()
        {

            var newsposts = from po in context.Postses
                            orderby po.CreateDate descending
                            select po;
            ViewBag.newsposts = newsposts.ToList();
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

            ViewBag.tag = tagList.ToList();
            ViewBag.sellist = clickList.ToList();
            ViewBag.newsposts = bydatePosts.ToList();
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
            ViewBag.newsposts = newsposts.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Contact(Message n)
        {
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

        [HttpPost]
        public ActionResult Single(Reply n, int id)
        {
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
    }
}