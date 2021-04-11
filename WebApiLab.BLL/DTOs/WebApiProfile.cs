using AutoMapper;
using System.Linq;

namespace WebApiLab.BLL.DTO
{
    public class WebApiProfile : Profile
    {
        public WebApiProfile()
        {
            CreateMap<DAL.Entities.Product, Product>()
                .ForMember(p=>p.Orders, 
                           opt=>opt.MapFrom(x=>x.ProductOrders.Select(po=> po.Order)))
                .ReverseMap();
            CreateMap<DAL.Entities.Order, Order>().ReverseMap();
            CreateMap<DAL.Entities.Category, Category>().ReverseMap();
        }
    }
}
