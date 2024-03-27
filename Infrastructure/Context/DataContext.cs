using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<CourseEntity> Courses { get; set; }
    public DbSet<SubscribeEntity> Subscribers { get; set; }
    public DbSet<ContactEntity> Contacts { get; set; }
}
