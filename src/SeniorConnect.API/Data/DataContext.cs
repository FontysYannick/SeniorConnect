using Microsoft.Extensions.Hosting;
using SeniorConnect.API.Entities;

namespace SeniorConnect.API.Data
{
    public partial class DataContext: DbContext
    {
        public DataContext() { }

        public DataContext(DbContextOptions<DataContext> options): base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Activity> Activities { get; set; }

        public virtual DbSet<ActivityUsers> ActivityUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Create unique index for email in user
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Define relationship between User and Activity
            modelBuilder.Entity<Activity>()
                .HasOne(a => a.Organizer)
                .WithMany(u => u.Activities)
                .HasForeignKey(a => a.OrganizerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Define primary key and auto-increment for ActivityUsers
            modelBuilder.Entity<ActivityUsers>()
                .HasKey(au => au.ActivityUserId);

            modelBuilder.Entity<ActivityUsers>()
                .Property(au => au.ActivityUserId)
                .ValueGeneratedOnAdd();

            // Define foreign key relationship between ActivityUsers and User
            modelBuilder.Entity<ActivityUsers>()
                .HasOne(au => au.User)
                .WithMany(u => u.ActivityUsers)
                .HasForeignKey(au => au.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Define foreign key relationship between ActivityUsers and Activity
            modelBuilder.Entity<ActivityUsers>()
                .HasOne(au => au.Activity)
                .WithMany(a => a.ActivityUsers)
                .HasForeignKey(au => au.ActivityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
