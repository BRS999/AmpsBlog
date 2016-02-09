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

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Post> Posts { get; set; }
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
    }
}