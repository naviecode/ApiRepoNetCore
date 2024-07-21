using ShopApi.Data.Infrastructure;
using ShopApi.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApi.Data.Repositories
{
    public interface IRoleRepository : IRepository<Roles>
    {
        
    }
    public class RoleRepository : RepositoryBase<Roles> , IRoleRepository
    {

        public RoleRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
