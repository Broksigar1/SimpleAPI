using Model;

namespace DatabaseService.Repositories
{
    public interface IProductRepository
    {
        Task AddProductAsync(Product product, CancellationToken cancellationToken);
        Task ChangeProductCountAsync(string productName, int count, CancellationToken cancellationToken);
        Task ChangeProductCategoryAsync(string productName, int newCategoryId, CancellationToken cancellationToken);
        Task DeleteProductAsync(string productName, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(string productName, CancellationToken cancellationToken);
        IAsyncEnumerable<ProductWithCategoryName> GetAllProductsAsync(CancellationToken cancellationToken);
    }
}

