using DAL.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Services.Interfaces;
using Services.Interfaces.Core;
using Services.Services;
using Services.Services.Core;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration().WriteTo.File("C:\\Log\\LibLog-.log", rollingInterval: RollingInterval.Day).CreateLogger();
#region Mapper
var config = new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AppMapper());
});
var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);
#endregion

#region dbContext-Inject
var connectionString = builder.Configuration.GetConnectionString("DevConnection");

builder.Services.AddDbContext<LibaryMDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});
#endregion


#region inject
builder.Services.AddScoped<IBusinessUnitOfWork, BusinessUnitOfWork>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBorrowedBookService, BorrowedBookService>();
#endregion


builder.Services.AddControllersWithViews();

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
