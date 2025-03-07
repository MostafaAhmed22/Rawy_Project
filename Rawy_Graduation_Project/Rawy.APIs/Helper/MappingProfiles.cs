﻿using AutoMapper;
using Rawy.APIs.Dtos;
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
		}
    }
}
