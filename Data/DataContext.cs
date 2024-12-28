using DATINGAPP.Entities;
using Microsoft.EntityFrameworkCore;

namespace DATINGAPP.Data
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<AppUser> Users { get; set; }
    }
}
