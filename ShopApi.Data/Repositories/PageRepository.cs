using ShopApi.Data.Infrastructure;
using ShopApi.Model.Models;

namespace ShopApi.Data.Repositories
{
    public interface IPageRepository : IRepository<Page>
    {
        
    }
    public class PageRepository : RepositoryBase<Page>, IPageRepository
    {
       
        public PageRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

       
    }
}
