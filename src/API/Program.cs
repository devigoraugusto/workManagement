using Application.DTOs;
using Application.Interfaces;
using Application.Mapping;
using Application.Services;
using Application.Validator;
using FluentValidation;
using Infra.Data;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("TaskDb"));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITaskService, TaskService>();

// Register AutoMapper and FluentValidation
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<TaskProfile>());

//builder.Services.AddAutoMapper(typeof(TaskProfile).Assembly);
//builder.Services.AddValidatorsFromAssemblyContaining<CreateTaskDtoValidator>();

builder.Services.AddScoped<IValidator<CreateTaskDto>, CreateTaskDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateTaskDto>, UpdateTaskDtoValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
