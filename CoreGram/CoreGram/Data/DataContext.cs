using CoreGram.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreGram.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Follower>().ToTable("Followers").HasKey(x => new { x.UserId, x.FollowerId });

            modelBuilder.Entity<User>().HasMany(x => x.UsersFollowers)
                                       .WithOne(x => x.UserFollower)
                                       .HasForeignKey(x => x.FollowerId)
                                       .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>().HasMany(x => x.UsersFollowings)
                           .WithOne(x => x.UserFollowing)
                           .HasForeignKey(x => x.UserId)
                           .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UsersProfiles { get; set; }
        public DbSet<Follower> Followers { get; set; }
    }
}
