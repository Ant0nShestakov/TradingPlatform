using AVS.Models.AddressModels;
using AVS.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AVS.Configurations.AddressConfigurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(role => role.Id);
            builder.HasIndex(role => role.Name).IsUnique();

            builder
                .Property(role => role.Id)
                .HasDefaultValueSql("NEWID()")
                .ValueGeneratedOnAdd();

            builder.HasIndex(role => role.Name).IsUnique();

            builder
                .HasMany(role => role.Permissions)
                .WithMany(permission => permission.Roles);

            builder
                .HasMany(role => role.Users)
                .WithMany(user => user.Roles);

        }
    }
}
