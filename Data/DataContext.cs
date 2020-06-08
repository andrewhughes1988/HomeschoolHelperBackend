using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using HomeschoolHelperApi.Models;

namespace HomeschoolHelperApi.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<User> Users { get; set; }

        public DataContext(DbContextOptions options)
        :base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Record>()
                .HasOne<User>(r => r.User)
                .WithMany(u => u.Records)
                .IsRequired()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Record>()
                .HasOne<Student>(r => r.Student)
                .WithMany(s => s.Records)
                .IsRequired()
                .HasForeignKey(r => r.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Record>()
                .HasOne<Subject>(r => r.Subject)
                .WithMany(s => s.Records)
                .IsRequired()
                .HasForeignKey(r => r.SubjectId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Student>()
                .HasOne<User>(s => s.User)
                .WithMany(u => u.Students)
                .IsRequired()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Subject>()
                .HasOne<User>(s => s.User)
                .WithMany(u => u.Subjects)
                .IsRequired()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}