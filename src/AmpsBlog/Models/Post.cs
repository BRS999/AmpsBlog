using System;

namespace AmpsBlog.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }

    }
}