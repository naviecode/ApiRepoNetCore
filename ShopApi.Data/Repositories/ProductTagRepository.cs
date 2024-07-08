using ShopApi.Data.Infrastructure;
using ShopApi.Model.Models;

namespace ShopApi.Data.Repositories
{
    public interface IProductTagRepository : IRepository<ProductTag>
    {
        
    }
    public class ProductTagRepository : RepositoryBase<ProductTag>, IProductTagRepository
    {
       
        public ProductTagRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

       
    }
}
