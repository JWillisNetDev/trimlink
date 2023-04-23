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

// Inject our LinkService
builder.Services.AddSingleton<ILinkService, LinkService>(sp =>
{
    UnitOfWorkFactory unitOfWorkFactory = new UnitOfWorkFactory(opts =>
    {
        if (builder.Environment.IsDevelopment())
        {
            opts.UseInMemoryDatabase("trimlink-devdb");
        }
    });

    return new LinkService(unitOfWorkFactory);
});

// TODO Add Authentication using jwt bearer authentication schema (maybe?)

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();

app.MapFallbackToFile("index.html");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();