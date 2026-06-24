namespace DatabaseService.Repositories
{
    public interface ICategoryRepository
    {
        /// <summary>
        /// Add new category
        /// </summary>
        Task AddCategoryAsync(string name, CancellationToken cancellationToken);

        /// <summary>
        /// Delete category
        /// </summary>
        Task DeleteCategoryAsync(string name, CancellationToken cancellationToken);

        /// <summary>
        /// Check is category exists
        /// </summary>
        Task<bool> ExistsAsync(string categoryName, CancellationToken cancellationToken);

        /// <summary>
        /// Check is category exists
        /// </summary>
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Return all categories
        /// </summary>
        IAsyncEnumerable<string> GetAllCategoriesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Rename category
        /// </summary>
        Task RenameCategoryAsync(string name, string newName, CancellationToken cancellationToken);
    }
}
