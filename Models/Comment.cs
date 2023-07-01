using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Models
{

    public class Comment
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public DateTime PostedAt { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
        [ForeignKey("UserId")] public ApplicationUser User { get; set; }
        [ForeignKey("PostId")] public Post Post { get; set; }

    }
}
