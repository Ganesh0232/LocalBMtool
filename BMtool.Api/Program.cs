using BMtool.Application.Interfaces;
using BMtool.Application.Mapper;
using BMtool.Application.Services;
using BMtool.Core.Entities;
using BMtool.Core.Repository;
using BMtool.Infrastructure.Data.Context;
using BMtool.Infrastructure.Repository;
using Microsoft.Data.SqlClient;
using System.Data;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//// add the service registration

builder.Services.AddSingleton<BMtoolContext>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddAutoMapper(typeof(BMtoolDtoMapper));

builder.Services.Configure<ConnectionStringOptions>(builder.Configuration.GetSection(ConnectionStringOptions.Position));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

