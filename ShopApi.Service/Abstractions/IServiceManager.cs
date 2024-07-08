namespace ShopApi.Service.Abstractions
{
    public interface IServiceManager
    {
        IPostCategoryService PostCategoryService { get; }
        IProductCategoryService ProductCategoryService { get; }
        IProductService ProductService { get; }
        IErrorService ErrorService { get; }
        IUserService UserService { get; }
        IAuthService AuthService { get; }
        IUploadFileService UploadFileService { get; }
    }
}
