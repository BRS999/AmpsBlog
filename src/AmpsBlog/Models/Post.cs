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
        public string Content { get; set; }
        public string Tags { get; set; }
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
        
        [Display(Name = "Status")]
        public int StatusId { get; set; }
        public PostStatus PostStatus { get; set; }

        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

        [Display(Name = "Blog")]
        public int BlogId { get; set; }
        public Blog Blog { get; set; }

    }



}