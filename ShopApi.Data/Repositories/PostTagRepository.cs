using ShopApi.Data.Infrastructure;
using ShopApi.Model.Models;

namespace ShopApi.Data.Repositories
{
    public interface IPostTagRepository : IRepository<PostTag>
    {
        
    }
    public class PostTagRepository : RepositoryBase<PostTag>, IPostTagRepository
    {
       
        public PostTagRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

       
    }
}
