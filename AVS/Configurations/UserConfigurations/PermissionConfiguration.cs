using AVS.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AVS.Configurations.AddressConfigurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(permission => permission.Id);
            builder.HasIndex(permission => permission.Name).IsUnique();

            builder.Property(p => p.Id)
                .HasDefaultValueSql("NEWID()")  // Автогенерация значения
                .ValueGeneratedOnAdd();  // Автоматическая генерация при добавлени

            builder.HasIndex(permission => permission.Name).IsUnique();

            builder
                .HasMany(permission => permission.Roles)
                .WithMany(role => role.Permissions);

        }
    }
}
