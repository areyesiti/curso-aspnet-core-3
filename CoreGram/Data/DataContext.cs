using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGram.Data.Configurations;
using CoreGram.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreGram.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserProfileConfiguration());
            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new FollowerConfiguration());
            modelBuilder.ApplyConfiguration(new LikeConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new PostCommentConfiguration());


            //// Configuración Fluent en método OnModelCreating del DbContext

            //// Configuramos la clave primaria
            //modelBuilder.Entity<User>().ToTable("Users").HasKey(x => x.Id);

            //// Configuramos la relación 1 a 1 con UserProfile
            //// y la regla de eliminación de borrado en cascada
            //modelBuilder.Entity<User>().HasOne<UserProfile>(x => x.Profile)
            //       .WithOne(s => s.User)
            //       .HasForeignKey<UserProfile>(x => x.Id)
            //       .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Follower> Followers { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<UserProfile> UsersProfiles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostComment> PostsComments { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
