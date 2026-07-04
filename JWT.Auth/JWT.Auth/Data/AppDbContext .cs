using JWT.Auth.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace JWT.Auth.Data
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        //protected override void OnModelCreating(
        //    ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>()
        //        .ToTable("users");

        //    modelBuilder.Entity<User>()
        //        .Property(x => x.Id)
        //        .HasColumnName("id");

        //    modelBuilder.Entity<User>()
        //        .Property(x => x.Username)
        //        .HasColumnName("username");

        //    modelBuilder.Entity<User>()
        //        .Property(x => x.Email)
        //        .HasColumnName("email");

        //    modelBuilder.Entity<User>()
        //        .Property(x => x.PasswordHash)
        //        .HasColumnName("password_hash");
        //}
    }
}
