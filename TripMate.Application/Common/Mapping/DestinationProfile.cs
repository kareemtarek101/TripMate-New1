using AutoMapper;
using TripMate.Application;
using TripMate.Infrastructure.Persistence.Entities;

namespace TripMate.Application.Common.Mapping
{
    public class DestinationProfile : Profile
    {
        public DestinationProfile()
        {
            CreateMap<Destination, DestinationDto>();
        }
    }
}
