namespace DCXAir.API.Application.Mappings
{
    using AutoMapper;
    using DCXAir.API.Domain.Entities;
    using DCXAir.API.Application.DTOs;
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
           
            CreateMap<Journey, JourneyDto>()
                .ForMember(dest => dest.Type, opt => opt.Ignore()) 
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Price)); 


            CreateMap<Flight, FlightDto>()
                .ForMember(dest => dest.Currency, opt => opt.Ignore()); 

            
            CreateMap<Transport, TransportDto>();
        }
    }
}