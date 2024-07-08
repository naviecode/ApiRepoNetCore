using ShopApi.Data.Infrastructure;
using ShopApi.Model.Models;

namespace ShopApi.Data.Repositories
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        
    }
    public class OrderDetailRepository : RepositoryBase<OrderDetail>, IOrderDetailRepository
    {
       
        public OrderDetailRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

       
    }
}
