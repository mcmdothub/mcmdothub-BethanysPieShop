using mcmdothub_BethanysPieShop.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Initial with MockData without DbContext
//builder.Services.AddScoped<ICategoryRepository, MockCategoryRepository>();
//builder.Services.AddScoped<IPieRepository, MockPieRepository>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPieRepository, PieRepository>();

// we invoke the GetCart method on ShoppingCart class passing in the service provider "sp"
// AddScoped is going to create a ShoppingCart for the request => all places within the request that have access to ShoppingCart will use 
// the same ShoppingCart that gets instantiated in the GetCart method
// so we'll use IShoppingCart and than we'll get back that scoped instance over the ShoppingCart
builder.Services.AddScoped<IShoppingCart, ShoppingCart>(sp => ShoppingCart.GetCart(sp));

// add sessions because we use sessions in the ShoppingCart
builder.Services.AddSession();

// AddHttpContextAccessor to be able to do "GetRequiredService<IHttpContextAccessor>" inside the GetCart method
builder.Services.AddHttpContextAccessor();

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

// add support for sessions (sessions require the use of middleware)
app.UseSession();

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
