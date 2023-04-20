using trimlink.data;
using trimlink.api.Configuration;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

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

// Inject our database DB context
builder.Services.AddDbContextFactory<TrimLinkDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.UseInMemoryDatabase("trimlink-devdb");
    }
    // TODO Create production database
});

// Add our static page content
builder.Services.AddSpaStaticFiles(options =>
{
    options.RootPath = "wwwroot";
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSpaStaticFiles();
app.UseSpa(bldr =>
{
    if (app.Environment.IsDevelopment())
    {
        bldr.UseProxyToSpaDevelopmentServer("http://localhost:8080");
    }
});

app.Run();