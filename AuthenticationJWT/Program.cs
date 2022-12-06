using AuthenticationJWT.Domain.Interfaces;
using AuthenticationJWT.Domain.Services;
using AuthenticationJWT.Infrastructure.Context;
using AuthenticationJWT.Infrastructure.Context.Entities;
using AuthenticationJWT.Infrastructure.Interfaces;
using AuthenticationJWT.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "You api title", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Aud"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddDbContext<AppDbContext>
        (o => o.UseInMemoryDatabase("MyDatabase"));
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();
var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetService<AppDbContext>();

// Data seed
var users = new List<User>
        {
            new User
            {
                Id= 1,
                Email = "daniel.carmona.jaramillo@gmail.com",
                Password = "123456",
                Name = "Daniel Carmona",
                Role = "Admin"
            },
            new User
            {
                Id= 2,
                Email = "andereEmail@mail.com",
                Password = "123456",
                Name = "Jemanden",
                Role = "Verkaufer"
            }
        };

db!.Users.AddRange(users);

db.SaveChanges();
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
