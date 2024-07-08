using ShopApi.Data.Infrastructure;
using ShopApi.Model.Models;

namespace ShopApi.Data.Repositories
{
    public interface IMenuRepository : IRepository<Menu>
    {
        
    }
    public class MenuRepository : RepositoryBase<Menu>, IMenuRepository
    {
       
        public MenuRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

       
    }
}
