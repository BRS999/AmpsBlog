using System;
using System.ComponentModel.DataAnnotations;

namespace AmpsBlog.Models
{
    public class Post
    {
        public Post()
        {
            Status = PostStatus.Draft;
            DateCreated = DateTime.UtcNow;
        }

        public int PostId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public string Tags { get; set; }
        public PostStatus Status { get; set; }
        public DateTime DateCreated { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }

    }

    [Flags]
    public enum PostStatus
    {
        Draft,
        Published,
        Archive
    }
}