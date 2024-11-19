using DCXAir.API.Application.Interfaces;
using DCXAir.API.Application.Services;
using DCXAir.API.Application.DTOs;
using DCXAir.Infrastructure.Data;
using DCXAir.API.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<List<Journey>>(provider =>
{
    try
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "markets.json");
        var flights = JourneyDataLoader.LoadFlights(filePath); 
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
        Console.WriteLine($"Error al cargar el archivo markets.json: {ex.Message}");
        return new List<Journey>();
    }
});

builder.Services.AddScoped<IJourneyService, JourneyService>();

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