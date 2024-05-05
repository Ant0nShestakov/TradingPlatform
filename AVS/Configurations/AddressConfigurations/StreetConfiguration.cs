using AVS.Models.AddressModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AVS.Configurations.AddressConfigurations
{
    public class StreetConfiguration : IEntityTypeConfiguration<Street>
    {
        public void Configure(EntityTypeBuilder<Street> builder)
        {
            builder.HasKey(street => street.Id);

            builder
                .HasOne(street => street.Locality)
                .WithMany(locality => locality.Streets)
                .HasForeignKey(street => street.LocalityID).OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(street => street.Addresses)
                .WithOne(address => address.Street)
                .HasForeignKey(address => address.StreetID).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
