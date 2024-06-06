using AVS.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AVS.Configurations.AddressConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(user => user.Id).HasDefaultValueSql("NEWID()");
            builder.HasKey(user => user.Id);

            builder.HasIndex(user => user.Email).IsUnique();

            //Пользователь - объявление
            builder.HasMany(user => user.Advertisements)
                .WithOne(advertisements => advertisements.User)
                .HasForeignKey(advertisements => advertisements.UserId).OnDelete(DeleteBehavior.Cascade);

            //Пользователь - роль
            builder.HasMany(user => user.Roles)
                .WithMany(role => role.Users);

            //Пользователь - сообщения
            builder.HasMany(user => user.Messages)
                .WithMany(message => message.Users);

        }
    }
}
