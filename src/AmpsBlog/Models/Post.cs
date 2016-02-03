using System;
using System.ComponentModel.DataAnnotations;

namespace AmpsBlog.Models
{
    public class Post
    {
        public Post()
        {
            Status = "Draft";
            DateCreated = DateTime.UtcNow;
        }

        public int PostId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public string Tags { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }

    }
}