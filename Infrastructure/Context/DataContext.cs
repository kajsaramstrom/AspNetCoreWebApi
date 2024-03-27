using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<CourseEntity> Courses { get; set; }
    public DbSet<SubscribeEntity> Subscribers { get; set; }
    public DbSet<ContactEntity> Contacts { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<SavedCourseEntity> SavedCourses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<SavedCourseEntity>()
            .HasKey(sc => new { sc.CourseId, sc.UserId });

        modelBuilder.Entity<SavedCourseEntity>()
            .HasOne(sc => sc.Course)
            .WithMany(c => c.SavedCourses)
            .HasForeignKey(sc => sc.CourseId);


        modelBuilder.Entity<SavedCourseEntity>()
            .HasOne(sc => sc.User)
            .WithMany(u => u.SavedCourses)
            .HasForeignKey(sc => sc.UserId);
    }
}
