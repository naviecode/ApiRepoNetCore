﻿using ShopApi.Data.Infrastructure;
using ShopApi.Model.Models;

namespace ShopApi.Data.Repositories
{
    public interface ISystemConfigRepository : IRepository<SystemConfig>
    {
        
    }
    public class SystemConfigRepository : RepositoryBase<SystemConfig>, ISystemConfigRepository
    {
       
        public SystemConfigRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

       
    }
}