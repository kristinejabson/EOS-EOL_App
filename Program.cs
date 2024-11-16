using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TimeBasedPreventiveMeasures.Data;
using TimeBasedPreventiveMeasures.Services;
using TimeBasedPreventiveMeasures.BackgroundTask;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Register the DbContext with the connection string from configuration
builder.Services.AddDbContext<LifecycleManagementDB>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LifecycleManagementDB"))
);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Register the EmailSender service
builder.Services.AddScoped<EmailSender>();

// Register the NotificationService and other services
builder.Services.AddScoped<INotificationService, NotificationService>();

// Register the background service
builder.Services.AddHostedService<NotificationBackgroundService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authentication}/{action=Index}/{id?}");

app.Run();
