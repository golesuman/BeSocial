using BlogApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    // public DbSet<ApplicationUser> Users { get;  set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Post> Posts { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)  
    {  
        base.OnModelCreating(builder);  
        // this.SeedUsers(builder);  
        this.SeedRoles(builder);
        // this.SeedUserRoles(builder);  
    }  
  
    private void SeedUsers(ModelBuilder builder)  
    {  
        ApplicationUser user = new ()  
        {  
            Id = "b74ddd14-6340-4840-95c2-db12554843e5",  
            UserName = "Admin",  
            Email = "admin@gmail.com",  
            LockoutEnabled = false,  
            PhoneNumber = "1234567890"  
        };  
  
        PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();  
        user.PasswordHash = passwordHasher.HashPassword(user, "Admin*123");  
  
        builder.Entity<ApplicationUser>().HasData(user);  
    }  
  
    private void SeedRoles(ModelBuilder builder)  
    {  
        builder.Entity<IdentityRole>().HasData(  
            new IdentityRole() { Id = "fab4fac1-c546-41de-aebc-a14da6895711", Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },  
            new IdentityRole() { Id = "c7b013f0-5201-4317-abd8-c211f91b7330", Name = "HR", ConcurrencyStamp = "2", NormalizedName = "Human Resource" }  
        );  
    }  
  
    private void SeedUserRoles(ModelBuilder builder)  
    {  
        builder.Entity<IdentityUserRole<string>>().HasData(  
            new IdentityUserRole<string>() { RoleId = "fab4fac1-c546-41de-aebc-a14da6895711", UserId = "b74ddd14-6340-4840-95c2-db12554843e5" }  
        );  
    }  
}