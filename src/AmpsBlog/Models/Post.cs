using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AmpsBlog.Models
{
    public class Post
    {
        public Post()
        {
            DateCreated = DateTime.UtcNow;
        }

        public int PostId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Permalink { get; set; }
        [Required]
        public string Content { get; set; }
        public string Tags { get; set; }
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
        
        [Display(Name = "Status")]
        public PostStatus PostStatus { get; set; }

        public ApplicationUser Author { get; set; }

        [Display(Name = "Blog")]
        public Blog Blog { get; set; }

    }



}