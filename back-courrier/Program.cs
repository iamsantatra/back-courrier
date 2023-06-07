using back_courrier.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using back_courrier.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<IUploadService, UploadService>();
builder.Services.AddScoped<ICourrierService, CourrierService>();
builder.Services.AddScoped<IUtilisateurService, UtilisateurService>();
// Add authentication services
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    //options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddCookie(IdentityConstants.ApplicationScheme, options =>
{
    // Configure cookie options
    options.Cookie.Name = "token";
    // Other cookie options...
});

builder.Services.AddDistributedMemoryCache();   
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(120);
});

var connectionString = builder.Configuration.GetConnectionString("LocalDatabase");
builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(connectionString));
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<ApplicationDbContext>();
/*    dbContext.Database.EnsureDeleted();
    dbContext.Database.EnsureCreated();*/
    // dbContext.Database.GenerateCreateScript();
    dbContext.Database.Migrate();
/*    dbContext.SeedData();*/
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

/*app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 403 || context.Response.StatusCode == 404)
    {
        context.Response.Redirect("/Error");
    }
});*/

app.Use(async (context, next) =>
{
    if (!context.User.Identity.IsAuthenticated && context.Request.Path != "/Index")
    {
        context.Response.Redirect("/Index");
        return;
    }

    await next();
});

app.MapRazorPages();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
});

app.Run();
