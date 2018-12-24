using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreWebApi.Entities
{
    public class UserConfiguration: IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            // builder.Property(x => x.UserName).IsRequired();
            builder.HasAlternateKey(x => x.UserName);
            builder.Property(x => x.Password).IsRequired();
        }
    }
}
