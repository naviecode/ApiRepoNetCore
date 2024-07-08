using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApi.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory dbFactory;
        private WebShopDbContext dbContext;
        public UnitOfWork(IDbFactory dbFactory, WebShopDbContext dbContext) 
        {
            this.dbFactory = dbFactory;
            this.dbContext = dbContext;
        }
        public WebShopDbContext DbContext { get { return dbContext; } }

        public void Commit()
        {
            dbContext.SaveChanges();
        }
    }
}
