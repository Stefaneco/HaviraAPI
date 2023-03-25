using System;
using AutoMapper;
using Azure.Storage.Blobs;
using HaviraApi.Entities;
using HaviraApi.Models.Dto;
using HaviraApi.Models;
using HaviraApi.Models.Response;

namespace HaviraApi.Mappers;

public class HaviraMappingProfile : Profile
{

    public HaviraMappingProfile()
	{
        CreateMap<Group, GroupListItemDto>()
            .ForMember(dest => dest.UserDtos, opt => opt.MapFrom(src => src.UserProfiles));
            //.ForMember(dest => dest.DishListItemDtos, opt => opt.MapFrom(src => src.Dishes));
        CreateMap<UserProfile, UserDto>();
        CreateMap<Dish, DishListItemDto>();
        CreateMap<Group, GroupDto>()
            .ForMember(dest => dest.UserDtos, opt => opt.MapFrom(src => src.UserProfiles))
            .ForMember(dest => dest.DishListItemDtos, opt => opt.MapFrom(src => src.Dishes));

    }
}

