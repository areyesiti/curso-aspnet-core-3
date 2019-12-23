using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGram.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreGram.Data.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            // Se configura la primary key
            builder.ToTable("Comments").HasKey(x => x.Id);

            // Configuramos la relación 1 a muchos con Comments
            // y la regla de eliminación de borrado en cascada
            builder.HasOne<User>(x => x.User)
                   .WithMany(x => x.Comments)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
