using AutoMapper;
using SalesOrderSystem_BackEnd.DTOs;
using SalesOrderSystem_BackEnd.Models;

namespace SalesOrderSystem_BackEnd.Profiles;
public class MappingProfile : Profile
{
  public MappingProfile()
  {
    // Existing mappings
    CreateMap<CreateSalesRequestDTO, SalesRequestModel>();
    CreateMap<SalesRequestLineDTO, SalesRequestLineModel>();
    CreateMap<SalesRequestModel, CreateSalesRequestDTO>();
    CreateMap<SalesRequestLineModel, SalesRequestLineDTO>();

    // Add this mapping for your grid DTO
    CreateMap<SalesRequestModel, SalesRequesterTableDTO>()
        .ForMember(dest => dest.SR, opt => opt.Ignore()) // You can set SR manually later if needed
        .ForMember(dest => dest.Actions, opt => opt.Ignore()) // Actions buttons handled in UI
        .ForMember(dest => dest.SalesDate, opt => opt.MapFrom(src => src.SalesDate.ToString("g"))); // Format date
  }
}
