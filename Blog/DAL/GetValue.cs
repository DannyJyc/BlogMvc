using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Blog.Models;

namespace Blog.DAL
{

    public class GetValue
    {
        static BlogContext context = new Models.BlogContext();
        public static List<Reply> ListReplySecond(int pid, int sid)
        {
            var list = from res in context.Replies
                       where res.PostsId == pid && res.SecondReply == sid
                       select res;
            return list.ToList();
        }

        public static List<Tag> Tag(int id)
        {
            var list = from po in context.Postses
                       from tag in po.Tags
                       where po.PostsId == id
                       select tag;
            var TagName = list.ToList();
            return TagName;
        }

        public static int TagNameId(string Name)
        {
            var list = from tag in context.Tags
                where tag.Name == Name
                select tag;
            return list.First().TagId;
        }
    }
}