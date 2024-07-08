using ShopApi.Data.Infrastructure;
using ShopApi.Model.Models;

namespace ShopApi.Data.Repositories
{
    public interface IMenuGroupRepository : IRepository<MenuGroup>
    {
       
    }
    public class MenuGroupRepository : RepositoryBase<MenuGroup>, IMenuGroupRepository
    {
       
        public MenuGroupRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

       
    }
}
