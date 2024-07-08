namespace ShopApi.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
