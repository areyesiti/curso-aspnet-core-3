using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGram.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreGram.Data.Configurations
{
    public class LikeConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            // Configuramos una primary key compuesta
            builder.ToTable("Likes").HasKey(x => new { x.PostId, x.UserId });

            // Configuramos la relación 1 a muchos con Likes
            // y la regla de eliminación de borrado en cascada
            builder.HasOne<User>(x => x.User)
                   .WithMany(x => x.Likes)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Configuramos la relación 1 a muchos con Post
            // y la regla de eliminación restrictiva para evitar errores en la eliminación
            builder.HasOne<Post>(x => x.Post)
                   .WithMany(x => x.Likes)
                   .HasForeignKey(x => x.PostId)
                   .OnDelete(DeleteBehavior.Restrict);     
        }
    }
}
