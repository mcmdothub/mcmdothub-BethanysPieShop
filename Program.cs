using mcmdothub_BethanysPieShop.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICategoryRepository, MockCategoryRepository>();
builder.Services.AddScoped<IPieRepository, MockPieRepository>();

// enable MVC in your application
builder.Services.AddControllersWithViews();

var app = builder.Build();

// look for incoming requests for static files(JPEG,CSS files etc) inside wwwroot folder
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    // diagnostic middleware component that may contain secret information that we don't want our users to see
    app.UseDeveloperExceptionPage();
}

// will enable our application to let MVC handle incoming requests on controllers
app.MapDefaultControllerRoute();

app.Run();
