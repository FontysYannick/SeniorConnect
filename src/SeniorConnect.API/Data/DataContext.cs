using Microsoft.Extensions.Hosting;
using SeniorConnect.API.Entities;

namespace SeniorConnect.API.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<ActivityUsers> ActivityUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //create unique for email in user
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();


            //Define relationship between User and Activity
            modelBuilder.Entity<Activity>()
                .HasOne(a => a.Organizer)
                .WithMany(u => u.Activities)
                .HasForeignKey(a => a.OrganizerId);

            // Define many-to-many relationship between User and Activity
            modelBuilder.Entity<ActivityUsers>()
                .HasKey(au => new { au.UserId, au.ActivityId });

            modelBuilder.Entity<ActivityUsers>()
                .HasOne(au => au.User)
                .WithMany(u => u.ActivityUsers)
                .HasForeignKey(au => au.UserId)
                .OnDelete(DeleteBehavior.Restrict); // or DeleteBehavior.NoAction

            modelBuilder.Entity<ActivityUsers>()
                .HasOne(au => au.Activity)
                .WithMany(a => a.ActivityUsers)
                .HasForeignKey(au => au.ActivityId)
                .OnDelete(DeleteBehavior.Restrict); // or DeleteBehavior.NoAction

        }
    }
}
