using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ECommerce.ViewModel;

namespace ECommerce.Models
{
	public class ApplicationDBContext : IdentityDbContext<User>
	{
		public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) 
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.Entity<User>().ToTable("Users", "Security");
			builder.Entity<IdentityRole>().ToTable("Roles", "Security");
			builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "Security");
			builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "Security");
			builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "Security");
			builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "Security");
			builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "Security");
		}

		public DbSet<Cart> Carts { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<CartProducts> FinalCarts { get; set; }
		public DbSet<Receipt> Receipts { get; set; }
		public DbSet<Shipping> Shippings { get; set; }
		public DbSet<productOptions> ProductOptions { get; set; }
		public DbSet<options> options { get; set; }
		public DbSet<CartPrdouctOptions> CartProductsOptions { get; set; }
	}
}
