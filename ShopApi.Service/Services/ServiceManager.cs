using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ShopApi.Data.Infrastructure;
using ShopApi.Data.Repositories;
using ShopApi.Service.Abstractions;
using ShopApi.Service.Models.AuthDto;

namespace ShopApi.Service.Services
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IPostCategoryService> _lazyPostCategory;
        private readonly Lazy<IProductCategoryService> _lazyProductCategory;
        private readonly Lazy<IProductService> _lazyProductService;
        private readonly Lazy<IErrorService> _lazyErrorService;
        private readonly Lazy<IUserService> _lazyUserService;
        private readonly Lazy<IRoleService> _lazyRoleService;
        private readonly Lazy<IAuthService> _lazyAuthService;
        private readonly Lazy<IUserContextService> _lazyUserContextService;
        private readonly Lazy<IUploadFileService> _lazyUploadFileService;

        public ServiceManager(IPostCategoryRepository postCategoryRepository, 
            IOptions<AppSettings> options,
            IHttpContextAccessor httpContextAccessor,
            IProductCategoryRepository productCategoryRepository, 
            IProductRepository productRepository,
            IProductTagRepository productTagRepository,
            ITagRepository tagRepository,
            IErrorRepository errorRepository, 
            IUserRepository userRepository,
            IUserTokenRepository userTokenRepository,
            IRoleRepository roleRepository,
            IUploadFileService uploadFileService,
            IUnitOfWork unitOfWork,
            IUserContextService userContextService,
            IMapper mapper) 
        {
            _lazyPostCategory = new Lazy<IPostCategoryService>(()=> new PostCategoryService(postCategoryRepository, unitOfWork));
            _lazyPostCategory = new Lazy<IPostCategoryService>(()=> new PostCategoryService(postCategoryRepository, unitOfWork));
            _lazyProductCategory = new Lazy<IProductCategoryService>(()=> new ProductCategoryService(productCategoryRepository, unitOfWork, uploadFileService, mapper, userContextService));
            _lazyProductService = new Lazy<IProductService>(() => new ProductService(productRepository, productTagRepository, tagRepository ,unitOfWork, uploadFileService, mapper, userContextService));
            _lazyUserService = new Lazy<IUserService>(() => new UserService(userRepository, unitOfWork, uploadFileService, mapper));
            _lazyRoleService = new Lazy<IRoleService> (() => new RoleService(roleRepository, unitOfWork, mapper, userContextService));
            _lazyAuthService = new Lazy<IAuthService>(() => new AuthService(options, userRepository, userTokenRepository, roleRepository, unitOfWork));
            _lazyUserContextService = new Lazy<IUserContextService>(()=> new UserContextService(httpContextAccessor));
            _lazyErrorService = new Lazy<IErrorService>(()=> new ErrorService(errorRepository, unitOfWork));
            _lazyUploadFileService = new Lazy<IUploadFileService>(() => new UploadFileService());
        }

        public IPostCategoryService PostCategoryService => _lazyPostCategory.Value;
        public IProductCategoryService ProductCategoryService => _lazyProductCategory.Value;
        public IProductService ProductService => _lazyProductService.Value;
        public IErrorService ErrorService => _lazyErrorService.Value;
        public IUserService UserService => _lazyUserService.Value;
        public IRoleService RoleService => _lazyRoleService.Value;
        public IAuthService AuthService => _lazyAuthService.Value;
        public IUserContextService UserContextService => _lazyUserContextService.Value;
        public IUploadFileService UploadFileService => _lazyUploadFileService.Value;
    }
}
