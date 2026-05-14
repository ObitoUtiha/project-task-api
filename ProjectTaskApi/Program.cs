using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProjectTaskApi.Common.Exceptions;
using ProjectTaskApi.Data;
using ProjectTaskApi.DTOs.Validator;
using ProjectTaskApi.Services;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog(
    (context, configuration) =>
    {
        configuration.ReadFrom.Configuration(
            context.Configuration);
    });

builder.Services.AddDbContext<ApplicationContext>(options =>
                                                    options.UseNpgsql(
                                                      builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITasksService, TasksService>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateProjectValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateTaskValidator>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration =
        builder.Configuration["Redis"];

    options.InstanceName = "ProjectTaskApi";
});

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    await dbContext.Database.MigrateAsync();
    await DbInitializer.SeedDataAsync(dbContext);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseAuthorization();

app.MapControllers();

app.Run();
