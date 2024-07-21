using FileService;
using FileService.Interface;
using Microsoft.EntityFrameworkCore;
using ScheduleCore.Entities;
using ScheduleService.Logic;
using ScheduleService.Logic.Interfaces;
using ScheduleService.Service;
using ScheduleService.Service.Interfaces;
using ScheduleWeb.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddTransient<ITimetableImporter, TimetableImporter>();
builder.Services.AddTransient<ITimetableService, TimetableService>();
builder.Services.AddTransient<IInputService, InputService>();
builder.Services.AddSingleton<IFileService, CsvFileService>();
builder.Services.AddSingleton<IFileService, JsonFileService>();
builder.Services.AddSingleton<IFileService, XmlFileService>();
builder.Services.AddDbContext<StudentManagementContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("Schedule")));
builder.Services.AddHttpClient();
builder.Services.AddScoped(typeof(ApiService));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
