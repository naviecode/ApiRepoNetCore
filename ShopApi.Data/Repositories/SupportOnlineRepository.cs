using ShopApi.Data.Infrastructure;
using ShopApi.Model.Models;

namespace ShopApi.Data.Repositories
{
    public interface ISupportOnlineRepository : IRepository<SupportOnline>
    {
        
    }
    public class SupportOnlineRepository : RepositoryBase<SupportOnline>, ISupportOnlineRepository
    {
       
        public SupportOnlineRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

       
    }
}
