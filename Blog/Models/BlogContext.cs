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
        public DbSet<User> Users { get; set; }
    }

    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }

        public ICollection<Posts> Postses { get; set; }
    }
}