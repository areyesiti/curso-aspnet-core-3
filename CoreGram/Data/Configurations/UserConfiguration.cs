using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreGram.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreGram.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {            
            // Configuramos la clave primaria
            builder.ToTable("Users").HasKey(x => x.Id);

            // Configuramos la relación 1 a 1 con UserProfile
            // y la regla de eliminación de borrado en cascada
            builder.HasOne<UserProfile>(x => x.Profile)
                   .WithOne(s => s.User)
                   .HasForeignKey<UserProfile>(x => x.Id)
                   .OnDelete(DeleteBehavior.Cascade);                  
        }
    }
}
