using AutoMapper;
using SalesOrderSystem_BackEnd.DTOs;
using SalesOrderSystem_BackEnd.Models;

namespace SalesOrderSystem_BackEnd.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {

            CreateMap<UsersModel, UserDTO>().ReverseMap();
            CreateMap<SalesRequestModel, SalesRequesterTableDTO>().ReverseMap();
           
        }
    }
}
