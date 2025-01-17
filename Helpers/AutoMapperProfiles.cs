using AutoMapper;
using DATINGAPP.DTOs;
using DATINGAPP.Entities;
using DATINGAPP.Extensions;

namespace DATINGAPP.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(
                    d => d.PhotoUrl,
                    o => o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain)!.Url)
                )
                .ForMember(d => d.Age, o => o.MapFrom(s => s.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
        }
    }
}
