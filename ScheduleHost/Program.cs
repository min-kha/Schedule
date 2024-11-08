using Microsoft.EntityFrameworkCore;
using ScheduleCore.Entities;
using ScheduleService.Service.Interfaces;
using ScheduleService.Service;
using FileService.Interface;
using FileService;
using ScheduleService.Logic.Interfaces;
using ScheduleService.Logic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ITimetableImporter, TimetableImporter>();
builder.Services.AddTransient<ITimetableService, TimetableService>();
builder.Services.AddTransient<IInputService, InputService>();
builder.Services.AddSingleton<IFileService, CsvFileService>();
builder.Services.AddSingleton<IFileService, JsonFileService>();
builder.Services.AddSingleton<IFileService, XmlFileService>();
builder.Services.AddDbContext<StudentManagementContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("Schedule")));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:5215")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseAuthorization();

app.MapControllers();

app.Run();
