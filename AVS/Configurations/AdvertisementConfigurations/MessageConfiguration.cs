using AVS.Models.AdvertisementModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AVS.Configurations.AdvertisementConfigurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(message => message.Id);

            builder
                .HasMany(message => message.Advertisements)
                .WithMany(advertisement => advertisement.Messages);

            builder
                .HasMany(message => message.Users)
                .WithMany(user => user.Messages);
        }
    }
}
