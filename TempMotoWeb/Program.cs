using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TempMotoWeb.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AquaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TempMotoWebContext") ?? throw new InvalidOperationException("Connection string 'TempMotoWebContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    Environment.SetEnvironmentVariable("baseUrl", "https://tempmotoweb.azurewebsites.net/");
    Environment.SetEnvironmentVariable("mapsKey", "AIzaSyDgBBPqSV-8pmM2ePnkPJa7U0mSdrP2dRs");
    Environment.SetEnvironmentVariable("apiKey", "AIzaSyAQGv7YbY1Y-SqsHH9sZBLQpy-yK7H_5-A");


    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    Environment.SetEnvironmentVariable("baseUrl", "https://localhost:44373/");
    Environment.SetEnvironmentVariable("mapsKey", "AIzaSyAiYs04uLWM_Eek7WuOorLA5A3Pic8pAuM");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Medicao}/{action=Index}/{id?}");

app.Run();
