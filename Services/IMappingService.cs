using SalesOrderSystem_BackEnd.DTOs;

namespace SalesOrderSystem_BackEnd.Services
{
   

        public interface IMappingService
        {
            TDestination Map<TDestination>(object source);

            JSONResponseDTO<TDestination> MapResponse<TSource, TDestination>(JSONResponseDTO<TSource> source);
        }
    
}
