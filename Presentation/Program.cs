using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DataAccess.DataContext;
using DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AttendanceContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AttendanceContext>();
builder.Services.AddControllersWithViews();
//builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

//registration of services with the services collection [so the application knows about the existence of these classes that might
//be asked later on by client classes.
//e.g. Controller (which is a client class) is asking for an instance of StudentsRepository (service class)

//Application knows about StudentRepository
//First time it is asked an instance is created
//Instance is stored in a pool depending on the method called below (AddScoped)
//whenever it is asked for again, instance is loaded from the pool of instances 

//normally we use AddScoped
//Scoped: it will create one instance per request;
//Transient: it will create an instance per request per call;
//Singleton: it will create ONE instance (e.g. StudentRepository instance) for the all the users, for all requests, and for all the calls;





builder.Services.AddScoped(typeof(StudentRepository));








var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
