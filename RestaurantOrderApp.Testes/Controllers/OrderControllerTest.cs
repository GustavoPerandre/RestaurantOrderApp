using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using RestaurantOrderApp.Controllers;
using RestaurantOrderApp.Data;
using RestaurantOrderApp.Data.DTOs;
using RestaurantOrderApp.Model;

namespace RestaurantOrderApp.Testes.Controllers
{
    public class OrderControllerTest
    {
        private readonly OrderContext _context;
        private readonly IMapper _mapper;

        public OrderControllerTest()
        {
            _context = A.Fake<OrderContext>();
            _mapper = A.Fake<IMapper>();
        }

        [Fact]
        public void OrderController_GetOrders_ReturnOk()
        {
            //Arrange
            var orders = A.Fake<ICollection<ReadOrderDTO>>();
            var orderList = A.Fake<List<ReadOrderDTO>>();
            A.CallTo(() => _mapper.Map<List<ReadOrderDTO>>(orders)).Returns(orderList);
            var controller = new OrderController(_context, _mapper);

            //Act
            var result = controller.GetOrders();

            //Assert
            result.Should().NotBeNull();
        }
    }
}
