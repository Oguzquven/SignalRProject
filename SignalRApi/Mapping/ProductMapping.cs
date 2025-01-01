using AutoMapper;
using SignalR.DtoLayer.DiscountDto;
using SignalR.DtoLayer.ProductDto;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Mapping
{
    public class ProductMapping:Profile
    {
        public ProductMapping()
        {
               CreateMap<Product,ResultProdutDto>().ReverseMap();
               CreateMap<Product,CreateProductDto>().ReverseMap();
               CreateMap<Product,UpdateProductDto>().ReverseMap();
               CreateMap<Product,GetProductDto>().ReverseMap();
               CreateMap<Product,ResultProductWithCategory>().ReverseMap();
        }
    }
}
