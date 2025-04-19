using Ambev.DeveloperEvaluation.Application.Sales.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.MappingProfiles
{
    public class SaleMappingProfile : Profile
    {
        public SaleMappingProfile()
        {
            CreateMap<Sale, SaleDto>();
            CreateMap<Sale, SaleDetailDto>();
            CreateMap<SaleItem, SaleItemDto>();
        }
    }
}