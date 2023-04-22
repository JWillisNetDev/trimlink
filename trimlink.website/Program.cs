using trimlink.data;
using trimlink.website.Configuration;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using shortid;
using trimlink.data.Repositories;
using trimlink.core.Services;
using trimlink.website;

var builder = WebApplication.CreateBuilder(args);

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

// Inject our UnitOfWork abstraction
//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(services =>
//{
//    DbContextOptionsBuilder<TrimLinkDbContext> optionsBuilder = new();
//    if (builder.Environment.IsDevelopment())
//    {
//        optionsBuilder.UseInMemoryDatabase("trimlink-devdb");
//    }
//    TrimLinkDbContext context = new TrimLinkDbContext(options: optionsBuilder.Options);
//    return new UnitOfWork(context);
//});

builder.Services.AddSingleton<ILinkService, LinkService>(sp =>
{
    DbContextOptionsBuilder<TrimLinkDbContext> optionsBuilder = new();
    if (builder.Environment.IsDevelopment())
    {
        optionsBuilder.UseInMemoryDatabase("trimlink-devdb");
    }

    UnitOfWorkFactory factory = new(optionsBuilder.Options);
    return new LinkService(factory);
});

// TODO Add Authentication using jwt bearer authentication schema

// Configure CORS to accept cross-origin requests from Vue frontend
builder.Services.AddCors(options =>
{
    // In-Development cors policy
    options.AddPolicy("FrontendCorsPolicy", bldr =>
    {
        bldr.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins("https://localhost:5001");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseCors("FrontendCorsPolicy");

app.UseStaticFiles();

app.MapFallbackToFile("index.html");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();