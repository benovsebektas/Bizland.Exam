using Bizland.Models;
using Microsoft.EntityFrameworkCore;

namespace Bizland.DAL
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
          public DbSet<Team> teams { get; set; }
       
    }

}
