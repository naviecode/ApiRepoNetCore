using ShopApi.Data.Infrastructure;
using ShopApi.Model.Models;

namespace ShopApi.Data.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
    }
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
