using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantOrderApp.Data;
using RestaurantOrderApp.Data.DTOs;
using RestaurantOrderApp.Data.Enums;
using RestaurantOrderApp.Model;

namespace RestaurantOrderApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private OrderContext _context;
        private IMapper _mapper;
        public OrderController(OrderContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult AddOrder([FromBody] CreateOrderDTO orderDTO)
        {
            Order order = _mapper.Map<Order>(orderDTO);
            
            order.dishList = convertOrderInPeriod(order);

            if (!validateOrder(order))
                return NotFound("Dish repetitions invalid for the selected period");

            _context.orders.Add(order);
            _context.SaveChanges();

            //In addition to the Created status(201), it shows where the resource was created(POSTMAN in Headers > Location of the call)
            return CreatedAtAction(nameof(RecoverOrderById), new { Id = order.Id }, order);
        }        

        [HttpGet]
        public IEnumerable<Order> RecoverOrders()
        {
            foreach (Order order in _context.orders)
            {
                order.dishList = groupedDishList(order);
            }
            return _context.orders;
        }        

        [HttpGet("{id}")]
        public IActionResult RecoverOrderById(int id)
        {
            Order order = _context.orders.FirstOrDefault(order => order.Id == id);

            if (order != null)
            {
                order.dishList = groupedDishList(order);
                ReadOrderDTO orderDTO = _mapper.Map<ReadOrderDTO>(order);
                return Ok(orderDTO);
            }
            else
                return NotFound();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOrderById(int id, [FromBody] UpdateOrderDTO orderDTO)
        {

            Order order = _context.orders.FirstOrDefault(order => order.Id == id);

            if (order == null)
                return NotFound();
            _mapper.Map(orderDTO, order);
            order.dishList = convertOrderInPeriod(order);
            
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrderById(int id)
        {
            Order order = _context.orders.FirstOrDefault(order => order.Id == id);

            if (order == null)
                return NotFound();

            _context.Remove(order);
            _context.SaveChanges();
            return NoContent();
        }

        private string[] splitedDishList(string dishList)
        {
            //Split dishList into an array of string separeted by commas
            string orders = dishList;
            string[] subs = orders.Split(',');
            return subs;
        }
        private string convertOrderInPeriod(Order order)
        {
            //Converts the order of a sequence of numbers to names, according to the time of day
            List<String> convertedOrder = new List<String>();
            string[] subs = splitedDishList(order.dishList);

            foreach (string sub in subs.OrderBy(x => x).ToList())
            {
                string dish = String.Empty;
                if (order.Period.ToUpper().Equals("MORNING"))
                    dish = Enum.Parse(typeof(EnumMorningDish), sub).ToString();
                else if (order.Period.ToUpper().Equals("NIGHT"))
                    dish = Enum.Parse(typeof(EnumNightDish), sub).ToString();

                //When the conversion fails, the numeric value is kept
                bool isNumeric = int.TryParse(dish, out int n);
                if (isNumeric)
                    dish = "error";
                convertedOrder.Add(dish);
            }

            //groups each item in the converted list, using a comma as a divisor, in alphabetic order
            return String.Join(",", convertedOrder);
        }
                
        private string groupedDishList(Order order)
        {
            //Groups dishes list items for better viewing
            List<String> convertedOrder = new List<String>();
            string[] subs = splitedDishList(order.dishList);

            foreach (var sub in subs.Distinct())
            {
                int count = subs.Where(x => x.Equals(sub)).Count();
                if (count > 1)
                    convertedOrder.Add(sub + "(x" + count.ToString() + ")");
                else
                    convertedOrder.Add(sub);
            }
            return String.Join(",", convertedOrder);
        }

        private bool validateOrder(Order order)
        {
            //validates the additional dishes rule within the same order
            //If it's morning, allow additional coffee; if ordered at night, allows additional potatoes.
            List<String> convertedOrder = new List<String>();
            string[] subs = splitedDishList(order.dishList);

            foreach (var sub in subs.Distinct())
            {
                int count = subs.Where(x => x.Equals(sub)).Count();
                if (count > 1 && order.Period.ToUpper() == "MORNING" && sub != "coffee")
                    return false;
                else if (count > 1 && order.Period.ToUpper() == "NIGHT" && sub != "potato")
                    return false;
            }

            return true;
        }
    }
}
