using DCXAir.API.Application.Interfaces;
using DCXAir.API.Application.Services;
using DCXAir.API.Infrastructure.Data;
using DCXAir.API.Application.Repositories;
using DCXAir.API.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using DCXAir.Infrastructure.Data;
using DCXAir.API.Application.Mappings;
using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.AddDbContext<JourneyDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddScoped<IJourneyRepository, JourneyRepository>();
builder.Services.AddScoped<IJourneyService, JourneyService>();
builder.Services.AddScoped<IJourneyDataLoader, JourneyDataLoader>();



builder.Services.AddSingleton<List<Journey>>(provider =>
{
    try
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "markets.json");
        var journeyDataLoader = provider.GetRequiredService<IJourneyDataLoader>();
        var flights = journeyDataLoader.LoadFlights(filePath);
        var journeys = flights
            .GroupBy(f => new { f.Origin, f.Destination })
            .Select(g => new Journey
            {
                Fligths = g.Select(f => new Flight
                {
                    Origin = f.Origin,
                    Destination = f.Destination,
                    Price = f.Price,
                    Transport = new Transport
                    {
                        FlightCarrier = f.Transport.FlightCarrier,
                        FlightNumber = f.Transport.FlightNumber
                    }
                }).ToList()
            })
            .ToList();

        return journeys;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error Loading markets.json: {ex.Message}");
        return new List<Journey>();
    }
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
