using IB_projekat.Certificates.Model;
using IB_projekat.Users.Model;
using Microsoft.EntityFrameworkCore;

namespace IB_projekat
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Certificate> Certificates { get; set; }


        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}
