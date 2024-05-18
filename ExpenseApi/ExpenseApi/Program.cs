using AutoMapper;
using BeireMKit.Authetication.Extensions;
using BeireMKit.Authetication.Models;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Mappings;
using ExpenseApi.Infra.Dependencies;
using ExpenseApi.Infra.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Config access mongoDB
var mongoDBConfig = builder.Configuration.GetSection("MongoDBSettings").Get<MongoDBConfig>();
builder.Services.AddSingleton(mongoDBConfig);

// Automapper
builder.Services.AddSingleton(new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new MappingProfileBaseId());
    cfg.AddProfile(new MappingProfileUser());
    cfg.AddProfile(new MappingProfileTransaction());
    cfg.AddProfile(new MappingProfileBankAccount());

}).CreateMapper());

// DependencyInjection
DependenciesInjector.Register(builder.Services);

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ExpenseApi", Version = "v1" });

    // Adiciona a documenta��o XML gerada
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Auth
var jwtSetthings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.ConfigureJwtAuthentication(jwtSetthings);
builder.Services.ConfigureJwtServices();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExpenseApi V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Middleware
app.UseMiddleware<ExceptionMiddleware>();

app.Run();

public partial class Program { }
