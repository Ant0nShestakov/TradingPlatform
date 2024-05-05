using AVS.Models.AdvertisementModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AVS.Configurations.AddressConfigurations
{
    public class AdvertisementPhotoConfiguration : IEntityTypeConfiguration<AdvertisementPhoto>
    {
        public void Configure(EntityTypeBuilder<AdvertisementPhoto> builder)
        {
            builder.HasKey(advertisementPhoto => advertisementPhoto.ID);

            builder.HasIndex(advertisementPhoto => advertisementPhoto.Path).IsUnique();

            builder
                .HasOne(advertisementPhoto => advertisementPhoto.Advertisement)
                .WithMany(advertisement => advertisement.Photos)
                .HasForeignKey(advertisementPhoto => advertisementPhoto.AdvertisementsId);
        }
    }
}
