using AVS.Configurations.AddressConfigurations;
using AVS.Configurations.AdvertisementConfigurations;
using AVS.Models.AddressModels;
using AVS.Models.AdvertisementModels;
using AVS.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace AVS.DB_Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;
        public DbSet<Permission> Permissions { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Advertisement> Advertisements { get; set; } = null!;
        public DbSet<AdvertisementPhoto> AdvertisementPhotos { get; set; } = null!;
        public DbSet<AdvertisementState> AdvertisementStates { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Address> Addressies { get; set; } = null!;
        public DbSet<Country> Countries { get; set; } = null!;
        public DbSet<Locality> Localities { get; set; } = null!;
        public DbSet<Region> Regions { get; set; } = null!;
        public DbSet<Street> Streets { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region AddressConfig
            modelBuilder.ApplyConfiguration(new AddressConfiguration());
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new LocalityConfiguration());
            modelBuilder.ApplyConfiguration(new RegionConfiguration());
            modelBuilder.ApplyConfiguration(new StreetConfiguration());
            #endregion

            #region AdvertisementConfig
            modelBuilder.ApplyConfiguration(new AdvertisementConfiguration());
            modelBuilder.ApplyConfiguration(new AdvertisementPhotoConfiguration());
            modelBuilder.ApplyConfiguration(new AdvertisementStateConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            #endregion

            #region UserConfig
            modelBuilder.ApplyConfiguration(new MessageConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            #endregion
        }
    }
}
