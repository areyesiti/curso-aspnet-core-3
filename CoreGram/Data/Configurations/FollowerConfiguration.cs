using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGram.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreGram.Data.Configurations
{
    public class FollowerConfiguration : IEntityTypeConfiguration<Follower>
    {
        public void Configure(EntityTypeBuilder<Follower> builder)
        {
            // Configuramos una primary key compuesta
            builder.ToTable("Followers").HasKey(x => new { x.UserId, x.FollowerId });

            // Configuramos la relación 1 a muchos con UsersFollowers
            // y la regla de eliminación restrictiva para evitar errores en la eliminación
            builder.HasOne<User>(d => d.UserFollower)
                .WithMany(p => p.UsersFollowers)
                .HasForeignKey(d => d.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuramos la relación 1 a muchos con UsersFollowings
            // y la regla de eliminación restrictiva para evitar errores en la eliminación
            builder.HasOne<User>(d => d.UserFollowing)
                .WithMany(p => p.UsersFollowings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
