using ShopApi.Data.Infrastructure;
using ShopApi.Model.Models;

namespace ShopApi.Data.Repositories
{
    public interface ISlideRepository : IRepository<Slide>
    {
        
    }
    public class SlideRepository : RepositoryBase<Slide>, ISlideRepository
    {
       
        public SlideRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

       
    }
}
