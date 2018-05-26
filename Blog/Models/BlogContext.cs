using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    internal class BlogContext:DbContext
    {
        public BlogContext()
            : base("name=BlogContext")
        {
        }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Posts> Postses { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}