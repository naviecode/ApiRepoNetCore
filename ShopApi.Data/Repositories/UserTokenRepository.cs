using ShopApi.Data.Infrastructure;
using ShopApi.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApi.Data.Repositories
{
    public interface IUserTokenRepository : IRepository<UserTokens>
    {
        
    }
    public class UserTokenRepository : RepositoryBase<UserTokens> , IUserTokenRepository
    {

        public UserTokenRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
