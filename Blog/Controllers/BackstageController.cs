using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;

namespace Blog.Controllers
{
    public class BackstageController : Controller
    {
        // GET: HomeCRUD
        BlogContext context = new BlogContext();
        public ActionResult Index()
        {
            var PostsList = from po in context.Postses
                select po;
            return View(PostsList);
        }

        [HttpGet]
        public ActionResult Add()
        {
            var TagList = from tag in context.Tags
                select tag;
            return View(TagList);
        }

        [HttpPost]
        public ActionResult Add(Posts n)
        {
            string Tag = Request["Tag"];
            var posts = new Posts();
            var tag = new Tag();
            posts.Title = n.Title;
            posts.Content = n.Content;
            posts.Outline = n.Outline;
            posts.CreateDate = DateTime.Now;
            posts.Click = 0;
            string[] tagarr = Tag.Split(',');
            foreach (var item in tagarr)
            {
                tag.Name = item;
                tag.Postses.Add(posts);
                context.Tags.Add(tag);
                //posts.TagId = n.TagId;
                context.SaveChanges();
            }
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult TagAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TagAdd(Tag n)
        {
            var tag = new Tag();
            tag.Name = n.Name;
            context.Tags.Add(tag);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var list = (from po in context.Postses
                where po.PostsId == id
                select po).First();
            return View(list);
        }

        [HttpPost]
        public ActionResult Edit(Posts n)
        {
            int id = Convert.ToInt16(Request["PostsId"]);
            var posts = context.Postses.SingleOrDefault(p => p.PostsId == id);
            posts.Title = n.Title;
            posts.Outline = n.Outline;
            posts.Content = n.Content;
            posts.Click = n.Click;
            posts.CreateDate = DateTime.Now;
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Del(int id)
        {
            var list = (from po in context.Postses
                where po.PostsId == id
                select po).SingleOrDefault();
            return View(list);
        }

        [HttpPost]
        public ActionResult Del()
        {
            int id = Convert.ToInt16(Request["PostsId"]);
            var del = context.Postses.Where(p => p.PostsId == id).FirstOrDefault();
            context.Postses.Remove(del);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}