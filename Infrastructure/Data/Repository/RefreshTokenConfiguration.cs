using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repository
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshToken");
            builder.HasKey(rt => rt.Id);
            builder.Property(rt => rt.Token).IsRequired();
            builder.Property(rt => rt.UserName).IsRequired().HasMaxLength(100);
            builder.HasIndex(rt => rt.Token).IsUnique();
        }
    }
}
