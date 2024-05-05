using AVS.Models.AddressModels;
using AVS.Models.AdvertisementModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AVS.Configurations.AddressConfigurations
{
    public class AdvertisementConfiguration : IEntityTypeConfiguration<Advertisement>
    {
        public void Configure(EntityTypeBuilder<Advertisement> builder)
        {
            builder.HasKey(advertisement => advertisement.ID);
            //Объявление - Состояние
            builder
                .HasOne(advertisement => advertisement.AdvertisementState)
                .WithMany(advertisementState => advertisementState.Advertisements)
                .HasForeignKey(advertisement => advertisement.AdvertisementStateId);

            //Объявление - Фото
            builder
                .HasMany(advertisement => advertisement.Photos)
                .WithOne(advertisementPhotos => advertisementPhotos.Advertisement)
                .HasForeignKey(advertisementPhotos => advertisementPhotos.AdvertisementsId);

            //Объявление - пользователь
            builder
                .HasOne(advertisement => advertisement.User)
                .WithMany(user => user.Advertisements)
                .HasForeignKey(advertisement => advertisement.UserId);

            //Объявление - Категории
            builder
                .HasMany(advertisement => advertisement.Categories)
                .WithMany(category => category.Advertisements);
        }
    }
}
