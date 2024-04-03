using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using CH.Simple.Entities;

namespace CH.Simple.EntityFrameworkCore.EntityTypeConfigurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(e => e.Name)
                    .HasColumnName("name");

            builder.Property(e => e.Mobile)
                .HasColumnName("mobile");

            builder.Property(e => e.Created)
                .HasColumnName("created");

            builder.Property(e => e.CreatedBy)
                .HasColumnName("create_by");

            builder.Property(e => e.Modified)
                .HasColumnName("modified");

            builder.Property(e => e.ModifiedBy)
                .HasColumnName("modifie_by");

            builder.Property(e => e.IsDelete)
                 .HasColumnName("is_delete");

        }
    }
}
