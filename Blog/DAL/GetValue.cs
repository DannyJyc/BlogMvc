using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

        public static bool WithoutRegisterName(string name)
        {
            bool t = false;
            var list = (from user in context.Users
                where user.Name == name
                select user).ToList();
            if (list.Count >= 1)
            {
                t = true;
            }
            return t;
        }

        public static string GetStrMd5(string ConvertString)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)));
            t2 = t2.Replace("-", "");
            return t2;
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

        public static string UserIdName(int id)
        {
            var list = (from user in context.Users
                where user.UserId == id
                select user).SingleOrDefault();
            return list.Name;
        }

        public static int TagNameId(string Name)
        {
            var list = from tag in context.Tags
                where tag.Name == Name
                select tag;
            return list.First().TagId;
        }

        public static bool IfTag(int id,string tag)
        {
            bool temp = false;
            var posts = from po in context.Postses
                from ta in po.Tags
                where po.PostsId == id
                select ta;
            foreach (var tagName in posts.ToList())
            {
                if (tagName.Name == tag)
                {
                    temp = true;
                    break;
                }
            }
            return temp;
        }
    }
}