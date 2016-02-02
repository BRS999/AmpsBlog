using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AmpsBlog.Models
{
    public class Blog
    {
        public Blog()
        {
            DateCreated = DateTime.UtcNow;
        }

        public int BlogId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string Url { get; set; }
        public List<Post> Posts { get; set; }
        public DateTime DateCreated { get; set; }
    }
}