using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Value> Values { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserFollowing> Followings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Value>()
                .HasData(
                    new Value { Id = 1, Name = "Value 1" },
                    new Value { Id = 2, Name = "Value 2" },
                    new Value { Id = 3, Name = "Value 3" }
                );

            builder.Entity<UserActivity>(builder => builder.HasKey(ua => new { ua.AppUserId, ua.ActivityId }));

            builder.Entity<UserActivity>()
                .HasOne(ua => ua.AppUser)
                .WithMany(a => a.UserActivities)
                .HasForeignKey(ua => ua.AppUserId);

            builder.Entity<UserActivity>()
                .HasOne(ua => ua.Activity)
                .WithMany(a => a.UserActivities)
                .HasForeignKey(ua => ua.ActivityId);

            builder.Entity<UserFollowing>(b =>
            {
                b.HasKey(x => new { x.ObserverId, x.TargetId });
                b.HasOne(x => x.Observer)
                    .WithMany(x => x.Followings)
                    .HasForeignKey(x => x.ObserverId)
                    .OnDelete(DeleteBehavior.Restrict);
                b.HasOne(x => x.Target)
                    .WithMany(x => x.Followers)
                    .HasForeignKey(x => x.TargetId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

        }
    }
}