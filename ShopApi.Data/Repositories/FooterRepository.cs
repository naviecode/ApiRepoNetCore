using ShopApi.Data.Infrastructure;
using ShopApi.Model.Models;
using System.Linq.Expressions;

namespace ShopApi.Data.Repositories
{
    public interface IFooterRepository : IRepository<Footer>
    {
        
    }
    public class FooterRepository : RepositoryBase<Footer>, IFooterRepository
    {
       
        public FooterRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

    }
}
