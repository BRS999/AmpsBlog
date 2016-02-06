using System.Collections.Generic;

namespace AmpsBlog.Models
{
    public class PostStatus
    {
        public int Id { get; set; }
        public string Status { get; set; }

        public List<Post> Posts { get; set; }
    }
}