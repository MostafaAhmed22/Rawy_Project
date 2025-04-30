using AutoMapper;
using Rawy.APIs.Dtos;
using Rawy.APIs.Dtos.AcoountDtos;
using Rawy.APIs.Dtos.StoryDtos;
using Rawy.DAL.Models;

namespace Rawy.APIs.Helper
{
    public class MappingProfiles : Profile
	{
        public MappingProfiles()
        {
			CreateMap<RegisterDto, AppUser>();
				//ForMember(dest => dest.UserName, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Email) && src.Email.Contains("@") ? src.Email.Split('@')[0] : string.Empty
	//)	));
			CreateMap<AppUser,UserDto>().ReverseMap();
			CreateMap<AppUser, WriterDto>().ReverseMap();

			CreateMap<Story,AddStoryDto>().ReverseMap()
				.ForMember(dest => dest.AppUser, opt => opt.Ignore()); //Ignore Writer navigation property;

			CreateMap<UpdateStoryDto, Story>().ReverseMap();

			CreateMap<RegisterDto, AppUser>().ReverseMap();
			CreateMap<CommentDto,Comment>().ReverseMap();
			CreateMap<RatingDto, Rating>().ReverseMap();
		}
    }
}
