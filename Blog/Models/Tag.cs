using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class Tag
    {
        public Tag()
        {
            Postses = new HashSet<Posts>();
        }
        public int TagId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Posts> Postses { get; set; }
    }
}