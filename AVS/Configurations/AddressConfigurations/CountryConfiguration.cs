using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using AVS.Models.AddressModels;

namespace AVS.Configurations.AddressConfigurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasKey(country => country.Id);

            builder
                .HasMany(country => country.Regions)
                .WithOne(region => region.Country)
                .HasForeignKey(region => region.CountryID);
        }
    }
}
