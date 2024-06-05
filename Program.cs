using System;
using System.Collections.Generic;
using System.Linq;
using Laboratorio_3.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Laboratorio_3.Controllers;
using Microsoft.AspNetCore.SignalR;





var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5000","https://localhost:5001", "http://*:5000", "https://*:5001");
var configuration = builder.Configuration;

builder.Services.AddControllers();



builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication()
.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = configuration["TokenAuthentication:Issuer"],
			ValidAudience = configuration["TokenAuthentication:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(
				configuration["TokenAuthentication:SecretKey"])),
		};
		options.Events = new JwtBearerEvents
		{
			OnMessageReceived = context =>
			{
				// Leer el token desde el query string
				var accessToken = context.Request.Query["access_token"];
				// Si el request es para el Hub u otra ruta seleccionada...
				var path = context.HttpContext.Request.Path;
				if (!string.IsNullOrEmpty(accessToken) &&
					(path.StartsWithSegments("/chatsegurohub") ||
					path.StartsWithSegments("/Propietarios/token")))
				{//reemplazar las urls por las necesarias ruta ⬆
					context.Token = accessToken;
				}
				return Task.CompletedTask;
			}
		};
	});
		


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(
options => options.UseMySql(
	configuration["ConnectionStrings:MySql"],
	ServerVersion.AutoDetect(configuration["ConnectionStrings:MySql"])
  )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseCors(x=>x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
    
app.UseAuthentication();	
app.UseAuthorization();
app.UseStaticFiles();

app.MapControllers();

app.Run();
