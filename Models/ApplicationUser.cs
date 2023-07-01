using Microsoft.AspNetCore.Identity;

namespace BlogApp.Models;

public class ApplicationUser : IdentityUser
{
    public ICollection<Post>? Posts {get; set;}
}