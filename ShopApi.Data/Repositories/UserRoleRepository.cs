using ShopApi.Data.Infrastructure;
using ShopApi.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApi.Data.Repositories
{
    public interface IUserRoleRepository : IRepository<UserRoles>
    {
        
    }
    public class UserRoleRepository : RepositoryBase<UserRoles> , IUserRoleRepository
    {

        public UserRoleRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
