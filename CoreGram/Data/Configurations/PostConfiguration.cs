using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGram.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreGram.Data.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            // Configuramos la primary key
            builder.ToTable("Posts").HasKey(x => x.Id);

            // Configuramos la relación 1 a muchos con Posts
            // y la regla de eliminación de borrado en cascada
            builder.HasOne<User>(x => x.User)
                   .WithMany(x => x.Posts)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
