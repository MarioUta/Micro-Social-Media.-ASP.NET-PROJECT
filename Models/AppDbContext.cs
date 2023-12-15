using Microsoft.EntityFrameworkCore;

namespace Mello.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups {  get; set; }
        public DbSet<Post> Posts {  get; set; }
    }
}
