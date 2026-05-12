using Microsoft.EntityFrameworkCore;
using ProjectTaskApi.Data;
using ProjectTaskApi.Services;
using FluentValidation;
using ProjectTaskApi.DTOs.Validator;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationContext>(options =>
                                                    options.UseNpgsql(
                                                      builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITasksService, TasksService>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateProjectValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateTaskValidator>();
builder.Services.AddFluentValidationAutoValidation();


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

app.UseAuthorization();

app.MapControllers();

app.Run();
