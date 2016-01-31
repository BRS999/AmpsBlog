using System;
using System.Collections.Generic;

namespace AmpsBlog.Models
{
    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }
        public List<Post> Posts { get; set; }
        public DateTime DateCreated { get; set; }
    }
}