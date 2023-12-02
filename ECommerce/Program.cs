using ECommerce.Models;
using ECommerce.Models.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMvc().AddNToastNotifyToastr(new ToastrOptions()
{
	ProgressBar = true,
	PositionClass = ToastPositions.TopRight,
	PreventDuplicates = true,
	CloseButton = true,
	ToastClass = "mt-5",
});

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

});

builder.Services.AddScoped<IOperations<Product>, ProductRepo>();
builder.Services.AddScoped<IOperations<Category>, CategoryRepo>();
builder.Services.AddScoped<IOperations<Shipping>, ShippingRepo>();

builder.Services.AddIdentity<User, IdentityRole>(x => x.Password.RequireNonAlphanumeric = false).AddEntityFrameworkStores<ApplicationDBContext>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapControllerRoute(
//                name: "AdminProduct",
//                pattern: "admin/product/{action}/{id?}",
//                defaults: new { controller = "AdminProduct", action = "Index" });

//app.MapControllerRoute(
//                name: "AdminDashBoard",
//                pattern: "admin/dashboard/{action}/{id?}",
//                defaults: new { controller = "AdminDashboard", action = "Index" });

//app.MapControllerRoute(
//                name: "AdminCategory",
//                pattern: "admin/category/{action}/{id?}",
//                defaults: new { controller = "Category", action = "Index" });

//app.MapControllerRoute(
//                name: "AdminShipping",
//                pattern: "admin/shipping/{action}/{id?}",
//                defaults: new { controller = "ShippingCompanies", action = "Index" });

app.MapControllerRoute(
				name: "AdminArea",
				pattern: "admin/[controller]/{action}/{id?}");

app.Run();
