using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGram.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreGram.Data.Configurations
{
    public class PostCommentConfiguration : IEntityTypeConfiguration<PostComment>
    {
        public void Configure(EntityTypeBuilder<PostComment> builder)
        {
            // Configuramos una primary key compuesta
            builder.ToTable("PostsComments").HasKey(x => new { x.PostId, x.CommentId });

            // Configuramos la relación 1 a muchos con Comments
            // y la regla de eliminación de borrado en cascada
            builder.HasOne<Comment>(x => x.Comment)
                   .WithMany(x => x.PostsComments)
                   .HasForeignKey(x => x.CommentId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Configuramos la relación 1 a muchos con Post
            // y la regla de eliminación restrictiva para evitar errores en la eliminación
            builder.HasOne<Post>(x => x.Post)
                   .WithMany(x => x.PostsComments)
                   .HasForeignKey(x => x.PostId)
                   .OnDelete(DeleteBehavior.Restrict);                
        }
    }
}
