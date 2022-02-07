using API.Entities;
using AutoMapper;
using API.DTOs;
using System.Linq;
using API.Extensions;

namespace API.Helpers
{
  public class AutoMapperProfiles : Profile
  {
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, MemberDto>()
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>
                src.Photos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => 
                src.DateOfBirth.CalculateAge()));

        CreateMap<AppUser, LikeDTO>()
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>
            src.Photos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src =>
            src.DateOfBirth.CalculateAge()));

        CreateMap<Photo, PhotoDto>();
        CreateMap<MemberUpdateDTO, AppUser>();
        CreateMap<UserRegisterDTO, AppUser>();
        CreateMap<Message, MessageDTO>()
          .ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom(
            src => src.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
          .ForMember(dest => dest.RecipientPhotoUrl, opt => opt.MapFrom(
            src => src.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url));
    }
  }
}