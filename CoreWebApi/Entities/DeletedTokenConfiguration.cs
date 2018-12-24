using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreWebApi.Entities
{
    public class DeletedTokenConfiguration: IEntityTypeConfiguration<DeletedToken>
    {
        public void Configure(EntityTypeBuilder<DeletedToken> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasAlternateKey(x => x.Jti);
            builder.Property(x => x.Exp).IsRequired();
        }
    }
}
