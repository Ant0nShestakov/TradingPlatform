using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using AVS.Models.AddressModels;

namespace AVS.Configurations.AddressConfigurations
{
    public class LocalityConfiguration : IEntityTypeConfiguration<Locality>
    {
        public void Configure(EntityTypeBuilder<Locality> builder)
        {
            builder.HasKey(locality => locality.ID);

            builder
                .HasOne(locality => locality.Region)
                .WithMany(region => region.Localitys)
                .HasForeignKey(locality => locality.RegionID);

            builder
                .HasMany(locality => locality.Streets)
                .WithOne(street => street.Locality)
                .HasForeignKey(street => street.LocalityID);
        }
    }
}
