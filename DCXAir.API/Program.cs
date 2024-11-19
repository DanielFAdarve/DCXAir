using DCXAir.API.Application.Interfaces;
using DCXAir.API.Application.Services;
using DCXAir.API.Application.DTOs;
using DCXAir.Infrastructure.Data;
using DCXAir.API.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<List<JourneyDto>>(provider =>
{
    try
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "markets.json");
        var flights = JourneyDataLoader.LoadFlights(filePath); // Cargar vuelos
        var journeyDtos = flights
            .GroupBy(f => new { f.Origin, f.Destination }) // Agrupar vuelos por origen y destino
            .Select(g => new JourneyDto
            {
                Type = "One Way", // O puedes usar una lógica más avanzada para determinar el tipo
                Flights = g.Select(f => new FlightDto
                {
                    Origin = f.Origin,
                    Destination = f.Destination,
                    Price = f.Price,
                    Transport = new List<TransportDto>
                    {
                        new TransportDto { FlightCarrier = f.Transport.FlightCarrier, FlightNumber = f.Transport.FlightNumber }
                    }
                }).ToList(),
                TotalPrice = g.Sum(f => f.Price)
            }).ToList();

        return journeyDtos;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al cargar el archivo markets.json: {ex.Message}");
        return new List<JourneyDto>();
    }
});

builder.Services.AddScoped<IJourneyService, JourneyService>();

// Configurar controladores
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar el pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();