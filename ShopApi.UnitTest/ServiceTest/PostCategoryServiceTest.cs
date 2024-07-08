using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShopApi.Data.Infrastructure;
using ShopApi.Data.Repositories;
using ShopApi.Model.Models;
using ShopApi.Service.Abstractions;
using ShopApi.Service.Services;
namespace ShopApi.UnitTest.ServiceTest
{

    [TestClass]
    public class PostCategoryServiceTest
    {
        private Mock<IPostCategoryRepository> _mockRepository;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private IPostCategoryService _categoryService;
        private List<PostCategory> _listCategory;
        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IPostCategoryRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _categoryService = new PostCategoryService(_mockRepository.Object, _mockUnitOfWork.Object);
            _listCategory = new List<PostCategory>()
            {
                new PostCategory(){ID = 1, Name = "category 1", Alias = "alias category 1", Status = true, Description = "desc 1", CreatedBy = "QS 1", MetaDesc = "1234", MetaKeyWord = "456", UpdatedBy = "quang son 1"},
                new PostCategory(){ID = 2, Name = "category 2", Alias = "alias category 2", Status = true, Description = "desc 2", CreatedBy = "QS 2", MetaDesc = "1234", MetaKeyWord = "456", UpdatedBy = "quang son 2"},
                new PostCategory(){ID = 3, Name = "category 3", Alias = "alias category 3", Status = true, Description = "desc 3", CreatedBy = "QS 3", MetaDesc = "1234", MetaKeyWord = "456", UpdatedBy = "quang son 3"}
            }    ;
        }
        [TestMethod]
        public void PostCateGory_Service_GetAll()
        {
            //setup method
            _mockRepository.Setup(m => m.GetAll(null)).Returns(_listCategory);

            //call Action
            var result = _categoryService.GetAll() as List<PostCategory>;

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
        }
        [TestMethod]
        public void PostCategory_Service_Create()
        {
            PostCategory category = new PostCategory();
            int id = 1;
            category.Name = "Test category";
            category.Alias = "test-category";
            category.Status = true;
            category.Description = "vi du";
            category.CreatedBy = "Quang son";
            category.MetaDesc = "123";
            category.MetaKeyWord = "456";
            category.UpdatedBy = "Quang son2";

            _mockRepository.Setup(m => m.Add(category)).Returns((PostCategory p) =>
            {
                p.ID = 1;
                return p;
            });

            var result = _categoryService.Add(category);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ID);
        }
    }
}
