namespace DCXAir.API.Domain.Entities
{
    public class Journey
    {
        public List<Flight> Fligths { get; set; } = new();
        public string Origin => Fligths.FirstOrDefault()?.Origin;
        public string Destination => Fligths.LastOrDefault()?.Destination;
        public double Price => Fligths.Sum(f => f.Price);  
    }
}
