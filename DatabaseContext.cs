﻿using IB_projekat.ActivationTokens.Model;
using IB_projekat.Certificates.Model;
using IB_projekat.PasswordResetTokens.Model;
using IB_projekat.Requests.Model;
using IB_projekat.SmsVerification.Model;
using IB_projekat.Users.Model;
using Microsoft.EntityFrameworkCore;


namespace IB_projekat
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<SmsVerificationCode> SmsVerificationCodes { get; set; }
        public DbSet<PasswordResetToken> passwordResetTokens { get; set; }
        public DbSet<Password> passwords { get; set; }

        public DbSet<ActivationToken> ActivationTokens { get; set; }


        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }

        public DatabaseContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Certificate>(entity =>
            {

                // make Issuer nullable
                entity.Property(e => e.Issuer)
                    .IsRequired(false) // make Issuer nullable
                    .HasDefaultValue(null);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
