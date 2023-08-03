using Microsoft.EntityFrameworkCore;
using TestProjectApi.Models;

namespace TestProjectApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions option):base(option)
        {

        }
        public DbSet<User> Users { get; set; }
    }
}
