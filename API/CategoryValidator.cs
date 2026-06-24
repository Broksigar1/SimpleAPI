using DatabaseService.Repositories;

namespace API
{
    public static class CategoryValidator
    {
        public static async Task ValidateAsync(string categoryName, ICategoryRepository repo, CancellationToken cancellationToken)
        {
            if (await repo.ExistsAsync(categoryName, cancellationToken))
                throw new ArgumentException($"Category with name = '{categoryName}' already exists");
        }

        public static async Task ValidateExistenceAsync(string categoryName, ICategoryRepository repo, CancellationToken cancellationToken)
        {
            if (!await repo.ExistsAsync(categoryName, cancellationToken))
                throw new ArgumentException($"Category with name = '{categoryName}' does not exist");
        }
    }
}
