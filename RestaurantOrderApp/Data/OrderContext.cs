using Microsoft.EntityFrameworkCore;
using RestaurantOrderApp.Model;

namespace RestaurantOrderApp.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> opt) : base(opt)
        {

        }

        public DbSet<Order> orders { get; set; }
    }
}