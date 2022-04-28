using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SmashMall.Shared.Models.Files;
using SmashMall.Shared.Models.Goods;
using SmashMall.Shared.Models.Items;
using SmashMall.Shared.Models.Orders;
using SmashMall.Shared.Models.Pickup;
using SmashMall.Shared.Models.UserAccount;

namespace SmashMall.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed database
            builder.Entity<IdentityRole>(b =>
            {
                b.HasData(
                    new IdentityRole
                    {
                        Name = "StandardUser",
                        NormalizedName = "STANDARDUSER",
                        Id = "586c3783-e5d6-42f9-a8c0-aa35b1e398bc",
                        ConcurrencyStamp = "be7d6c49-4be9-4824-a088-e6d78662b420"
                    });
                b.HasData(
                    new IdentityRole
                    {
                        Name = "Manager",
                        NormalizedName = "MANAGER",
                        Id = "59076949-79e0-4d11-b704-3c6c86483d52",
                        ConcurrencyStamp = "d26c7d01-a511-485d-a231-27633431fc92"
                    });
                b.HasData(
                    new IdentityRole
                    {
                        Name = "Admin",
                        NormalizedName = "ADMIN",
                        Id = "c9feeaa2-d0a3-4fab-8fed-bee9a7057134",
                        ConcurrencyStamp = "43ee1880-59e2-4954-8d94-7c8753ecea5e"
                    });
            });
            // End seed database

            builder.Entity<Good>(b =>
            {
                b.HasAlternateKey(k => k.Code);
                b.HasMany(x => x.Images).WithOne(x => x.Good).HasForeignKey(x => x.GoodId);
                b.HasMany(x => x.KeyFeatures).WithOne(x => x.Good).HasForeignKey(x => x.GoodId);
                b.HasMany(x => x.Feedback).WithOne(x => x.Good).HasForeignKey(x => x.GoodId);
                b.HasMany(x => x.Orders).WithOne(x => x.Good).HasForeignKey(x => x.GoodId);
                b.HasMany(x => x.Documents).WithOne(x => x.Good).HasForeignKey(x => x.GoodId);
            });

            builder.Entity<GoodCategory>(b =>
            {
                b.HasAlternateKey(k => k.Name);
                b.HasMany(k => k.Subcategories).WithOne(x => x.GoodCategory).HasForeignKey(x => x.GoodCategoryId).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<GoodSubcategory>(b =>
            {
                b.HasMany(k => k.Brands).WithOne(x => x.Subcategory).HasForeignKey(x => x.SubcategoryId).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Town>(b =>
            {
                b.HasMany(x => x.PickupStations).WithOne(x => x.Town).HasForeignKey(x => x.TownId);
                b.HasAlternateKey(x => x.Name);
            });

            builder.Entity<GoodBrand>(b =>
            {
                b.HasMany(x => x.Goods).WithOne(x => x.Brand).HasForeignKey(x => x.BrandId);
            });

        }
        public DbSet<GoodCategory> GoodCategory { get; set; }

        public DbSet<GoodSubcategory> GoodSubcategory { get; set; }

        public DbSet<Good> Good { get; set; }

        public DbSet<GoodImage> GoodImage { get; set; }

        public DbSet<GoodSubcategoryImage> GoodSubcategoryImages { get; set; }

        public DbSet<GoodOrder> GoodOrder { get; set; }

        public DbSet<GoodSaved> GoodsSaved { get; set; }

        public DbSet<GoodCart> GoodsCart { get; set; }

        public DbSet<GoodKeyFeature> GoodsKeyFeatures { get; set; }

        public DbSet<GoodBrand> GoodBrands { get; set; }

        public DbSet<GoodBrandImage> GoodBrandImages { get; set; }

        public DbSet<CustomerFeedback> CustomersFeedback { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<TopDeal> TopDeals { get; set; }

        public DbSet<PickupStation> PickupStations { get; set; }

        public DbSet<RejectedGood> RejectedGoods { get; set; }

        public DbSet<GoodDocument> GoodDocuments { get; set; }
    }
}
