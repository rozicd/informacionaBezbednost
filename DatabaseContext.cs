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
            modelBuilder.Entity<Admin>()
                .HasBaseType<AuthenticatedUser>()
                .HasDiscriminator<string>("UserType")
                .HasValue<Admin>("Admin");

            modelBuilder.Entity<AuthenticatedUser>()
                .HasBaseType<User>()
                .HasDiscriminator<string>("UserType")
                .HasValue<AuthenticatedUser>("Authorized");

            modelBuilder.Entity<UnauthenticatedUser>()
                .HasBaseType<User>()
                .HasDiscriminator<string>("UserType")
                .HasValue<UnauthenticatedUser>("Unauthorized");

            base.OnModelCreating(modelBuilder);
        }
    }
}
