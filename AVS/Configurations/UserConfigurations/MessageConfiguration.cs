using AVS.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AVS.Configurations.AddressConfigurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(message => message.Id);

            builder
                .HasOne(message => message.UserSender)
                .WithMany(user => user.Messages)
                .HasForeignKey(user => user.UserSenderId);

            builder
                .HasOne(message => message.UserReciver)
                .WithMany(user => user.Messages)
                .HasForeignKey(user => user.UserReciverId);
        }
    }
}
