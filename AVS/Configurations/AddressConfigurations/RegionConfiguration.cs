using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using AVS.Models.AddressModels;

namespace AVS.Configurations.AddressConfigurations
{
    public class RegionConfiguration : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> builder)
        {
            builder.HasKey(region => region.ID);
            builder.HasIndex(region => region.Name).IsUnique();

            builder
                .Property(region => region.ID)
                .HasDefaultValueSql("NEWID()")
                .ValueGeneratedOnAdd();

            builder
                .HasOne(region => region.Country)
                .WithMany(country => country.Regions)
                .HasForeignKey(region => region.CountryID).OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(region => region.Localitys)
                .WithOne(locality => locality.Region)
                .HasForeignKey(locality => locality.RegionID).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
