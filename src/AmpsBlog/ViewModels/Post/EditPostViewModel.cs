using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AmpsBlog.ViewModels.Post
{
    public class EditPostViewModel
    {
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
        public int PostStatus { get; set; }

        public string Author { get; set; }

        [Display(Name = "Blog")]
        public int Blog { get; set; }
    }
}
