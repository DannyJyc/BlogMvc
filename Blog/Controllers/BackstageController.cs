using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.DAL;
using Blog.Models;

namespace Blog.Controllers
{
    public class BackstageController : Controller
    {
        // GET: HomeCRUD
        BlogContext context = new BlogContext();
        public ActionResult Index()
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
            int id = Convert.ToInt16(Session["UserId"]);
            var PostsList = from po in context.Postses
                            where po.UserId == id
                            select po;
            if (GetValue.PerFour(groupid))
            {
                PostsList = from po in context.Postses
                            select po;
            }

            ViewBag.groupid = groupid;
            return View(PostsList);
        }

        public ActionResult Ueditor()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Add()
        {
            int groupid = Convert.ToInt16(Session["GroupId"]);
            if (Session["Name"] == null)
            {
                return Redirect("/Users/Login");
            }
            else if (!GetValue.PerThree(groupid))
            {
                return Redirect("/Backstage/Ero");
            }
            var TagList = from tag in context.Tags
                          select tag;
            ViewBag.tag = TagList.Select(p => p.Name).Distinct().Take(20).ToList();
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Add(string Title,string Outline,string Contents,string Tag,string Tags)
        {
            if (Session["Name"] == null)
            {
                return Redirect("/Users/Login");
            }

            if (Title == "" || Title == null)
            {
                return Content("titleNull");
            }
            if (Outline == "" || Outline == null)
            {
                return Content("outlineNull");
            }
            if (Contents == "" || Contents == null)
            {
                return Content("contentNull");
            }
            string TagPlus;
            if (Tag == "" || Tag == null)
            {
                TagPlus = Tags;
            }
            else if (Tags == "" || Tags == null)
            {
                TagPlus = Tag;
            }
            else
            {
                TagPlus = Tag + "," + Tags;
            }


            var posts = new Posts();
            var tag = new Tag();
            posts.Title = Title;
            posts.Content = Contents;
            posts.Outline = Outline;
            posts.CreateDate = DateTime.Now;
            posts.Click = 0;
            posts.UserId = Convert.ToInt16(Session["UserId"]);
            if (TagPlus == null || TagPlus == "")
            {
                context.Postses.Add(posts);
                context.SaveChanges();
            }
            else
            {
                string[] tagarr = TagPlus.Split(',');
                foreach (var item in tagarr)
                {
                    tag.Name = item;
                    tag.Postses.Add(posts);
                    context.Tags.Add(tag);
                    //posts.TagId = n.TagId;
                    context.SaveChanges();
                }
            }
            return Content("suc");
        }

        [HttpGet]
        public ActionResult TagAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TagAdd(Tag n)
        {
            if (Session["Name"] == null)
            {
                return Redirect("/Home/Index");
            }
            var tag = new Tag();
            tag.Name = n.Name;
            context.Tags.Add(tag);
            context.SaveChanges();
            return Redirect("/Backstage/Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            int groupid = Convert.ToInt16(Session["GroupId"]);
            if (!GetValue.PerThree(groupid))
            {
                return Redirect("/Backstage/Ero");
            }
            int userid = Convert.ToInt16(Session["UserId"]);
            var list = (from po in context.Postses
                where po.PostsId == id && po.UserId == userid
                select po).FirstOrDefault();
            if (GetValue.PerFour(groupid))
            {
                list = (from po in context.Postses
                    where po.PostsId == id
                    select po).First();
            }

            if (list == null)
            {
                return Content("滚！");
            }


            var tagList = from ta in context.Tags
                          select ta;
            ViewBag.tag = tagList.Select(p => p.Name).Distinct().Take(20).ToList();
            return View(list);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(Posts n)
        {
            if (Session["Name"] == null)
            {
                return Redirect("/Users/Login");
            }
            int id = n.PostsId;
            var posts = context.Postses.SingleOrDefault(p => p.PostsId == id);
            posts.Title = n.Title;
            posts.Outline = n.Outline;
            posts.Content = n.Content;
            posts.Click = n.Click;
            posts.CreateDate = DateTime.Now;
            context.SaveChanges();
            return Redirect("/Backstage/Index");
        }

        [HttpGet]
        public ActionResult Del(int id)
        {
            int groupid = Convert.ToInt16(Session["GroupId"]);
            if (!GetValue.PerThree(groupid))
            {
                return Redirect("/Backstage/Ero");
            }
            var list = (from po in context.Postses
                        where po.PostsId == id
                        select po).SingleOrDefault();
            return View(list);
        }

        [HttpPost]
        public ActionResult Del()
        {
            if (Session["Name"] == null)
            {
                return Redirect("/Users/Login");
            }
            int id = Convert.ToInt16(Request["PostsId"]);
            var del = context.Postses.Where(p => p.PostsId == id).FirstOrDefault();
            context.Postses.Remove(del);
            context.SaveChanges();
            return Redirect("/Backstage/Index");
        }

        public ActionResult Ero()
        {
            return View();
        }
    }
}