using KoperasiTenterApp.Models;
using Microsoft.EntityFrameworkCore;

namespace KoperasiTenterApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
    }
}
