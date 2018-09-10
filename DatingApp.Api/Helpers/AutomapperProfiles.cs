using System;
using System.Linq;
using AutoMapper;
using DatingApp.Api.Dtos;
using DatingApp.Api.Models;

namespace DatingApp.Api.Helpers
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<User, UserForListDto>()
                .ForMember(dto => dto.PhotoUrl, opt => {
                    opt.MapFrom(source => source.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dto => dto.Age, opt => {
                    opt.ResolveUsing(source => source.DateOfBirth.GetAge(DateTime.Now));
                });
            CreateMap<User, UserForDetailedDto>()
                .ForMember(dto => dto.PhotoUrl, opt => {
                    opt.MapFrom(source => source.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dto => dto.Age, opt => {
                    opt.ResolveUsing(source => source.DateOfBirth.GetAge(DateTime.Now));
                });
            CreateMap<Photo, PhotosForDetailedDto>();
            CreateMap<UserForUpdateDto, User>();
            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<PhotoForCreationDto, Photo>();
        }
    }
}