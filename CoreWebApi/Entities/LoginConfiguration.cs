using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Entities
{
    public class LoginConfiguration : IEntityTypeConfiguration<Login>
    {
        public void Configure(EntityTypeBuilder<Login> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasAlternateKey(x => x.UserName);
            builder.Property(x => x.Password).IsRequired();
            builder.Property(x => x.Company).IsRequired();
            builder.Property(x => x.Type).IsRequired();
        }
    }
}
