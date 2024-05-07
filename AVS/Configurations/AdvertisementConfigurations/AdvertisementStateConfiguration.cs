using AVS.Models.AdvertisementModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AVS.Configurations.AddressConfigurations
{
    public class AdvertisementStateConfiguration : IEntityTypeConfiguration<AdvertisementState>
    {
        public void Configure(EntityTypeBuilder<AdvertisementState> builder)
        {
            builder.HasKey(advertisementState => advertisementState.ID);
            builder.HasIndex(state => state.Name).IsUnique();

            builder
                .Property(state => state.ID)
                .HasDefaultValueSql("NEWID()")
                .ValueGeneratedOnAdd();

            builder.HasIndex(advertisementState => advertisementState.Name).IsUnique();

            builder
                .HasMany(advertisementState => advertisementState.Advertisements)
                .WithOne(advertisement => advertisement.AdvertisementState)
                .HasForeignKey(advertisement => advertisement.AdvertisementStateId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
