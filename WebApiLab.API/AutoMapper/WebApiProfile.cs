using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiLab.API
{
    public class WebApiProfile : Profile
    {
        public WebApiProfile()
        {
            CreateMap<Entities.Product, DTO.Product>()
                .ForMember(dto => dto.Orders, opt => opt.Ignore())
                .AfterMap((p, dto, ctx) =>
                    dto.Orders = p.ProductOrders.Select(po =>
                    ctx.Mapper.Map<DTO.Order>(po.Order)).ToList())
                .ReverseMap();
            CreateMap<Entities.Order, DTO.Order>().ReverseMap();
            CreateMap<Entities.Category, DTO.Category>().ReverseMap();
        }
    }
}
