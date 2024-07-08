using ShopApi.Data.Infrastructure;
using ShopApi.Model.Models;

namespace ShopApi.Data.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        
    }
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
       
        public OrderRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

       
    }
}
