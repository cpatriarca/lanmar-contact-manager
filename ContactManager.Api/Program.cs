using ContactManager.Application;
using ContactManager.Infrastructure;
using ContactManager.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//add services to the container.
builder.Services.AddControllersWithViews();

//configure EF Core
builder.Services.AddDbContext<ContactDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

//dependency Injection
builder.Services.AddScoped<IContactService, ContactService>();

var app = builder.Build();

//run any pending migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ContactDbContext>();
    dbContext.Database.Migrate();
}

//seed the database if it is empty
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ContactDbContext>();
    context.Database.EnsureCreated();

    if (!context.Contacts.Any())
    {
        context.Contacts.AddRange(
            new Contact { FirstName = "Alice", LastName = "Smith", CompanyName = "Acme", Mobile = "0412345678", Email = "alice@acme.com" },
            new Contact { FirstName = "Bob", LastName = "Jones", CompanyName = "Beta Corp", Mobile = "0498765432", Email = "bob@beta.com" },
            new Contact { FirstName = "Christian", LastName = "Patriarca", CompanyName = "Lanmar? :)", Mobile = "0466303347", Email = "xtianpatriarca@gmail.com" },
            new Contact { FirstName = "Marie", LastName = "Wartanian", CompanyName = "MV Artistry", Mobile = "0493514417", Email = "mvartanian@hotmail.com" },
            new Contact { FirstName = "Eva", LastName = "Patriarca", CompanyName = "Cutie Co.", Mobile = "0412345678", Email = "eva@cutieco.com" }
        );

        context.SaveChanges();
    }
}


//configure the HTTP request pipeline.
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Contact}/{action=Index}/{id?}");

app.Run();