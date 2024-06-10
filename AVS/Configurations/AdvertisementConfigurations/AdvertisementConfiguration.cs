using AVS.Models.AdvertisementModels;
using AVS.Models.UserModels;
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
                .HasForeignKey(advertisement => advertisement.AdvertisementStateId).OnDelete(DeleteBehavior.Restrict);

            //Объявление - Фото
            builder
                .HasMany(advertisement => advertisement.Photos)
                .WithOne(advertisementPhotos => advertisementPhotos.Advertisement)
                .HasForeignKey(advertisementPhotos => advertisementPhotos.AdvertisementsId).OnDelete(DeleteBehavior.Cascade);

            //Объявление - пользователь
            builder
                .HasOne(advertisement => advertisement.User)
                .WithMany(user => user.Advertisements)
                .HasForeignKey(advertisement => advertisement.UserId).OnDelete(DeleteBehavior.Cascade);

            //Объявление - Категории
            builder
                .HasOne(advertisement => advertisement.Category)
                .WithMany(category => category.Advertisements)
                .HasForeignKey(advertisement => advertisement.CategoryId);

            //Объявление - Адрес
            builder
                .HasOne(advertisement => advertisement.Address)
                .WithMany(address => address.Advertisements)
                .HasForeignKey(advertisement => advertisement.AddressId).OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(advertisement => advertisement.Messages)
                .WithOne(message => message.Advertisement)
                .HasForeignKey(message => message.AdvertisementId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
