using ShopApi.Data.Infrastructure;
using ShopApi.Model.Models;

namespace ShopApi.Data.Repositories
{
    public interface ITagRepository : IRepository<Tag>
    {
        
    }
    public class TagRepository : RepositoryBase<Tag>, ITagRepository
    {
       
        public TagRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

       
    }
}
