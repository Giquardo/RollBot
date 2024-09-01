using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

using RollBotApi.Services;
using RollBotApi.DataContext;
using RollBotApi.Models;
using RollBotApi.Configuration;
using RollBotApi.Repositories;
using RollBotApi.Controllers;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // add MongoDB settings 
        var mongoSettings = builder.Configuration.GetSection("MongoConnection");
        builder.Services.Configure<DatabaseSettings>(mongoSettings);
        builder.Services.AddSingleton<IMongoContext, MongoContext>();

        // Register the repositories
        builder.Services.AddSingleton<IUserRepository, UserRepository>();

        // Register the services
        builder.Services.AddTransient<IUserService, UserService>();

        // Register the logging service
        builder.Services.AddSingleton<ILoggingService>(new LoggingService("Logs/custom_log.txt"));

        // Register the controllers
        builder.Services.AddControllers();

        // Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // swagger
        app.MapSwagger();
        app.UseSwaggerUI();

        app.MapGet("/", () => Results.Redirect("/swagger"));

        app.MapControllers();


        app.Run("http://localhost:3000");
    }
}