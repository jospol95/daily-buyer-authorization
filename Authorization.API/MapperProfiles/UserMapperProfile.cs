using Authorization.API.Models;
using Authorization.API.ViewModels;
using AutoMapper;

namespace Authorization.API.MapperProfiles
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            //UserViewModel
            CreateMap<UserModel,UserViewModel>();
            CreateMap<UserViewModel,UserModel>();
            
            //NewUserViewModel
            CreateMap<UserViewModel,NewUserViewModel>();
            CreateMap<NewUserViewModel,UserViewModel>();
            
            //NewExternalUserViewModel
            CreateMap<UserViewModel,NewExternalUserViewModel>();
            CreateMap<NewExternalUserViewModel,UserViewModel>();
        }
    }
}