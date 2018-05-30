using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class Posts
    {
        public Posts()
        {
            Tags = new HashSet<Tag>();
        }
        public int PostsId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Outline { get; set; }
        public DateTime CreateDate { get; set; }
        public int Click { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }
        public User Users { get; set; }


        public virtual ICollection<Tag> Tags { get; set; }
        public ICollection<Reply> Replys { get; set; }
    }
}