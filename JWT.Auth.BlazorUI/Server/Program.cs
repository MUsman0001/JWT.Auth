using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using JWT.Auth.BlazorUI.Shared.Models;
using JWT.Auth.BlazorUI.Server.data;
using JWT.Auth.BlazorUI.Server.Service;
using JWT.Auth.BlazorUI.Shared.tokens;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "BlazorCors",
        policy =>
        {
            policy.WithOrigins("https://localhost:7256").AllowAnyHeader()
            .AllowAnyMethod();
        });
});
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7045/") });

builder.Services.AddDbContext<MyWorldDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyWorldDbConnection")));

builder.Services.AddScoped<StaffService>();
builder.Services.AddScoped<LabTestService>();



var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors("BlazorCors");

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
