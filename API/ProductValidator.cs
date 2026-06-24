using DatabaseService.Repositories;
using Model;

namespace API
{
    public static class ProductValidator
    {
        public static async Task ValidateAsync(
            Product product, 
            IProductRepository productRepo,
            ICategoryRepository categoryRepo, 
            CancellationToken cancellationToken)
        {
            if (!await categoryRepo.ExistsAsync(product.CategoryId, cancellationToken))
                throw new ArgumentException($"Category with id = {product.CategoryId} doesn`t exist");

            await ValidateProductAlreadyExist(product.Name, productRepo, cancellationToken);
        }

        public static async Task ValidateProductAlreadyExist(string name, IProductRepository repo, CancellationToken cancellationToken)
        {
            if (await repo.ExistsAsync(name, cancellationToken))
                throw new ArgumentException($"Product with the name = {name} already exists");
        }

        public static async Task ValidateProductExistence(string name, IProductRepository repo, CancellationToken cancellationToken)
        {
            if (!await repo.ExistsAsync(name, cancellationToken))
                throw new ArgumentException($"Product with the name = {name} does not exist");
        }
    }
}
