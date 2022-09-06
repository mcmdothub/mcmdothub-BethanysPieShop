using mcmdothub_BethanysPieShop.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Initial with MockData without DbContext
//builder.Services.AddScoped<ICategoryRepository, MockCategoryRepository>();
//builder.Services.AddScoped<IPieRepository, MockPieRepository>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPieRepository, PieRepository>();

// enable MVC in your application
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BethanysPieShopDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:BethanysPieShopDbContextConnection"]);
});

// application bulder
var app = builder.Build();

// look for incoming requests for static files(JPEG,CSS files etc) inside wwwroot folder
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    // diagnostic middleware component that may contain secret information that we don't want our users to see
    app.UseDeveloperExceptionPage();
}

// will enable our application to let MVC handle incoming requests on controllers
app.MapDefaultControllerRoute();            // "{controller=Home}/{action=Index}/{id?}"

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

// pass in app (application builder) in the Seed method
DbInitializer.Seed(app);

app.Run();
