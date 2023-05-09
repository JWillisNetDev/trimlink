using trimlink.data;
using trimlink.website.Configuration;
using Microsoft.EntityFrameworkCore;
using trimlink.core.Services;
using Serilog;
using trimlink.website;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Setup Serilog logger
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

// Create our CORS policy
const string myAllowFrontend = "myAllowFrontend";

builder.Services.AddCors(config =>
{
    if (builder.Environment.IsDevelopment())
        config.AddPolicy(myAllowFrontend, options =>
        {
            options
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
        });
    else
        config.AddPolicy(myAllowFrontend, options =>
        {
            options
                .WithOrigins("https://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    
});

// Add Serilog for logging to file (prod) and terminal (dev)
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

// Inject our TrimLinkDbContext as a scoped service
builder.Services.AddDbContext<TrimLinkDbContext>(options =>
{
    options.UseSqlServer(builder
            .Configuration
            .GetConnectionString("defaultConnection"),
        config => config.MigrationsAssembly("trimlink.data"));
});

// Inject our LinkService
builder.Services.AddScoped<ILinkService, LinkService>(sp =>
{
    TrimLinkDbContext dbContext = sp.GetRequiredService<TrimLinkDbContext>();
    UnitOfWork unitOfWork = new UnitOfWork(dbContext);
    return new LinkService(unitOfWork);
});

// TODO Add Authentication using jwt bearer authentication schema (maybe?)

var app = builder.Build();

// Create a scope to apply our database migrations.
using (var service = app.Services.CreateScope())
{
    await using var context = service.ServiceProvider.GetRequiredService<TrimLinkDbContext>();
    await context.Database.MigrateAsync();
}

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

app.UseCors(myAllowFrontend);

app.MapFallbackToFile("index.html");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();