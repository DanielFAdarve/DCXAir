

namespace DCXAir.Infrastructure.Data;
    
    using System.Text.Json;
    using DCXAir.API.Domain.Entities;
    public class JourneyDataLoader
    {
        public static List<Flight> LoadFlights(string filePath)
        {
            try
            {

                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"File not found in PATH : {filePath}");
                }

                var json = File.ReadAllText(filePath);

                if (string.IsNullOrWhiteSpace(json))
                {
                    throw new InvalidDataException("JSON File is empty.");
                }

                return JsonSerializer.Deserialize<List<Flight>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true
                }) ?? new List<Flight>();
            }
            catch (FileNotFoundException ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                throw;
            }
            catch (JsonException ex)
            {
                Console.Error.WriteLine($"Error processing JSON: {ex.Message}");
                throw new InvalidDataException("Invalid file format.", ex);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                throw;
            }
        }
}