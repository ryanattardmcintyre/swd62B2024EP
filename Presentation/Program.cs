using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DataAccess.DataContext;
using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc.Authorization;
using Presentation.ActionFilters;
using Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AttendanceContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<CustomUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AttendanceContext>();
//IdentityUser (IdentityRole) - built-in classes which facilitate the management of users in the database

//alternatively you can create your own model e.g. User



builder.Services.AddControllersWithViews();

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new LogActionFilter());  
});


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

 
builder.Services.AddScoped(typeof(GroupRepository));
builder.Services.AddScoped(typeof(SubjectRepository));
builder.Services.AddScoped(typeof(AttendanceRepository));
 
//The service is called ILogRepository BUT since i have 3 different versions, i need to pass also the implementation
 
int setting = builder.Configuration.GetValue<int>("logSetting");

switch (setting)
{
    case 1:
        builder.Services.AddScoped<ILogRepository, LogDBRepository>();
        builder.Services.AddScoped<IStudentRepository, StudentRepository>();
        break;

    case 2:
        builder.Services.AddScoped<ILogRepository, LogFileRepository>(x=> new LogFileRepository("logs.json"));
        builder.Services.AddScoped<IStudentRepository, StudentXmlRepository>();
        break;

    case 3:
        builder.Services.AddScoped<ILogRepository, LogCloudRepository>();
        break;
    default:
        builder.Services.AddScoped<ILogRepository, LogDBRepository>();
        break;
}

 




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
