﻿using AVS.Models.AdvertisementModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AVS.Configurations.AddressConfigurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(category => category.Id);
            builder.HasIndex(category => category.Name).IsUnique();

            builder
                .Property(category => category.Id)
                .HasDefaultValueSql("NEWID()")
                .ValueGeneratedOnAdd();

            builder.HasIndex(category => category.Name).IsUnique();

            builder
                .HasMany(category => category.Advertisements)
                .WithOne(advertisement => advertisement.Category)
                .HasForeignKey(advertisement => advertisement.CategoryId);
        }
    }
}
