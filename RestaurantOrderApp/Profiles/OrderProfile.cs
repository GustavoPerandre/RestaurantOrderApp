using AutoMapper;
using RestaurantOrderApp.Data.DTOs;
using RestaurantOrderApp.Model;

namespace RestaurantOrderApp.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<CreateOrderDTO, Order>();
            CreateMap<Order, ReadOrderDTO>();
            CreateMap<UpdateOrderDTO, Order>();
        }
    }
}
