using Microsoft.Extensions.Options;
using ShopApi.Data.Infrastructure;
using ShopApi.Data.Repositories;
using ShopApi.Service.Models;
using ShopApi.Service.Abstractions;

namespace ShopApi.Service.Services
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IPostCategoryService> _lazyPostCategory;
        private readonly Lazy<IProductCategoryService> _lazyProductCategory;
        private readonly Lazy<IProductService> _lazyProductService;
        private readonly Lazy<IErrorService> _lazyErrorService;
        private readonly Lazy<IUserService> _lazyUserService;
        private readonly Lazy<IAuthService> _lazyAuthService;
        private readonly Lazy<IUploadFileService> _lazyUploadFileService;

        public ServiceManager(IPostCategoryRepository postCategoryRepository, 
            IOptions<AppSettings> options,
            IProductCategoryRepository productCategoryRepository, 
            IProductRepository productRepository,
            IProductTagRepository productTagRepository,
            ITagRepository tagRepository,
            IErrorRepository errorRepository, 
            IUserRepository userRepository,
            IUploadFileService uploadFileService,
            IUnitOfWork unitOfWork) 
        {
            _lazyPostCategory = new Lazy<IPostCategoryService>(()=> new PostCategoryService(postCategoryRepository, unitOfWork));
            _lazyPostCategory = new Lazy<IPostCategoryService>(()=> new PostCategoryService(postCategoryRepository, unitOfWork));
            _lazyProductCategory = new Lazy<IProductCategoryService>(()=> new ProductCategoryService(productCategoryRepository, unitOfWork, uploadFileService));
            _lazyProductService = new Lazy<IProductService>(() => new ProductService(productRepository, productTagRepository, tagRepository ,unitOfWork, uploadFileService));
            _lazyUserService = new Lazy<IUserService>(() => new UserService(userRepository, unitOfWork, uploadFileService));
            _lazyAuthService = new Lazy<IAuthService>(() => new AuthService(options, userRepository, unitOfWork));
            _lazyErrorService = new Lazy<IErrorService>(()=> new ErrorService(errorRepository, unitOfWork));
            _lazyUploadFileService = new Lazy<IUploadFileService>(() => new UploadFileService());
        }

        public IPostCategoryService PostCategoryService => _lazyPostCategory.Value;
        public IProductCategoryService ProductCategoryService => _lazyProductCategory.Value;
        public IProductService ProductService => _lazyProductService.Value;
        public IErrorService ErrorService => _lazyErrorService.Value;
        public IUserService UserService => _lazyUserService.Value;
        public IAuthService AuthService => _lazyAuthService.Value;
        public IUploadFileService UploadFileService => _lazyUploadFileService.Value;
    }
}
