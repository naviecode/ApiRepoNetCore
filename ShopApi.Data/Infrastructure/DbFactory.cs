using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApi.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private WebShopDbContext dbContext;
        public DbFactory(WebShopDbContext dbContext)
        {
            this.dbContext = dbContext;

        }

        public WebShopDbContext Init()
        {
            return dbContext;
        }
        protected override void DisposeCore()
        {
            if(dbContext != null)
            {
                dbContext.Dispose();
            }
        }
    }
}
