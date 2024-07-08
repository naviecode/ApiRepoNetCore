using ShopApi.Data.Infrastructure;
using ShopApi.Model.Models;

namespace ShopApi.Data.Repositories
{
    public interface IPostCategoryRepository : IRepository<PostCategory>
    {
        
    }
    public class PostCategoryRepository : RepositoryBase<PostCategory>, IPostCategoryRepository
    {
       
        public PostCategoryRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

       
    }
}
