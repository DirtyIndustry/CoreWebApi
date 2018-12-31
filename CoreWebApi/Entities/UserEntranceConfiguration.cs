using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreWebApi.Entities
{
    public class UserEntranceConfiguration: IEntityTypeConfiguration<UserEntrance>
    {
        public void Configure(EntityTypeBuilder<UserEntrance> builder)
        {
            builder.HasKey(x => x.Id);
            // builder.Property(x => x.UserName).IsRequired();
            builder.HasAlternateKey(x => x.UserName);
            builder.Property(x => x.Password).IsRequired();
            builder.HasOne(u => u.Company).WithMany(c => c.Users).HasForeignKey(u => u.CompanyId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(u => u.User).WithOne(e => e.UserEntrance).HasForeignKey<User>(u => u.UserEntranceId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
