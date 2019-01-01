using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Entities
{
    public class CompanyEntranceConfiguration : IEntityTypeConfiguration<CompanyEntrance>
    {
        public void Configure(EntityTypeBuilder<CompanyEntrance> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasAlternateKey(x => x.CompanyName);
        }
    }
}
