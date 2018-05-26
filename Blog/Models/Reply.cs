using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class Reply
    {
        public int  ReplyId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ReplyContent { get; set; }
        public DateTime CreateDate { get; set; }
        public int SecondReply { get; set; }

        [ForeignKey("Postses")]
        public int PostsId { get; set; }
        public Posts Postses { get; set; }
    }
}