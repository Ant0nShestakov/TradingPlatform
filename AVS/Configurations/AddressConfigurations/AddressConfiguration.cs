using AVS.Models.AddressModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AVS.Configurations.AddressConfigurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(address => address.ID);

            builder
                .HasOne(address => address.Street)
                .WithMany(street => street.Addresses)
                .HasForeignKey(address => address.StreetID).OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(address => address.Advertisements)
                .WithOne(address => address.Address)
                .HasForeignKey(addres => addres.Address).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
