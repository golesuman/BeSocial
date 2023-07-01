using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Models;

public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string? PostImage { get; set; }
    public DateTime PublishedAt { get; set; }
    public string UserId { get; set; }
    public int CategoryId { get; set; }
    [ForeignKey("UserId")]
    public ApplicationUser User { get; set; }
    [ForeignKey("CategoryId")]
    public Category Category { get; set; }
    public ICollection<Comment>? Comments { get; set; }
}
