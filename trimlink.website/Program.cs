using trimlink.data;
using trimlink.website.Configuration;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using shortid;
using trimlink.data.Repositories;
using trimlink.core.Services;
using trimlink.website;
using Serilog;
using Serilog.AspNetCore;

// Configure Serilog immediately to enable logging during the configuration portion of application lifetime
using ILogger = Serilog.ILogger;

IConfiguration config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json")
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(config)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Create our CORS policy
const string _MyAllowFrontend = "MyAllowFrontend";

builder.Services.AddCors(options =>
{
    if (builder.Environment.IsDevelopment())
        options.AddPolicy(_MyAllowFrontend, options =>
        {
            options
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
        });
    else
        options.AddPolicy(_MyAllowFrontend, options =>
        {
            options
                .WithOrigins("https://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    
});

// Add SeriLog for logging to file (prod) and terminal (dev)
builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add AutoMapper loading our mapper configuration for DTO mapping
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<MappingProfile>();
});

// Inject our LinkService
builder.Services.AddSingleton<ILinkService, LinkService>(sp =>
{
    UnitOfWorkFactory unitOfWorkFactory = new UnitOfWorkFactory(opts =>
    {
        if (builder.Environment.IsDevelopment())
        {
            opts.UseInMemoryDatabase("trimlink-devdb");
        }

        if (builder.Environment.IsProduction())
        {
            opts.UseSqlServer();
        }
    });

    return new LinkService(unitOfWorkFactory);
});

builder.Services.AddCors();

// TODO Add Authentication using jwt bearer authentication schema (maybe?)

var app = builder.Build();

app.UseStaticFiles();

// Add request logging via Serilog.
// Do this before MapControllers() but after UseStaticFiles() to reduce unnecessary log noise.
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseCors(_MyAllowFrontend);

app.MapFallbackToFile("index.html");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();