using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class ViewModel
    {
        public List<Posts> Postses { get; set; }
        public List<Tag> Tags { get; set; }
    }
}