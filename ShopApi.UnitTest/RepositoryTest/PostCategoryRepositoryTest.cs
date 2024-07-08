using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopApi.Data;
using ShopApi.Data.Infrastructure;
using ShopApi.Data.Repositories;
using ShopApi.Model.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;


namespace ShopApi.UnitTest.RepositoryTest
{
    [TestClass]
    public class PostCategoryRepositoryTest
    {
        IDbFactory dbFactory;
        IUnitOfWork unitOfWork;
        IPostCategoryRepository postCategoryRepository;
        private WebShopDbContext dbContext;
        [TestInitialize()]
        public void Initialize() 
        {
            //Hàm này chạy đầu tiên
            var options = new DbContextOptionsBuilder<WebShopDbContext>()
           .UseInMemoryDatabase(databaseName: "WebExample")
           .Options;

            dbContext = new WebShopDbContext(options);
            dbFactory = new DbFactory(dbContext);
            postCategoryRepository = new PostCategoryRepository(dbFactory);
            unitOfWork = new UnitOfWork(dbFactory, dbContext);

        }
        [TestMethod]
        public void PostCategory_Repository_GetAll()
        {
            var list = postCategoryRepository.GetAll().ToList();
            Assert.AreEqual(0, list.Count);
        }


        [TestMethod]
        public void PostCategory_Repository_Create()
        {
        
            PostCategory category = new PostCategory();
            category.Name = "Test category";
            category.Alias = "test-category";
            category.Status = true;
            category.Description = "vi du";
            category.CreatedBy = "Quang son";
            category.MetaDesc = "123";
            category.MetaKeyWord = "456";
            category.UpdatedBy = "Quang son2";
            var result = postCategoryRepository.Add(category);
            unitOfWork.Commit();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ID);
        }
    }
}
