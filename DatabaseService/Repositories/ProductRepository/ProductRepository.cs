using Dapper;
using Model;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace DatabaseService.Repositories
{
    public class ProductRepository : Repository, IProductRepository
    {
        public ProductRepository(DbConnectionCreator connectionCreator) : base(connectionCreator)
        {
        }

        public async Task AddProductAsync(Product product, CancellationToken cancellationToken)
        {
            var command = new CommandDefinition(
                "insert into dbo.Products (CategoryId, Name, Count) values (@CategoryId, @Name, @Count)",
                new
                {
                    product.CategoryId,
                    product.Name,
                    product.Count,
                },
                cancellationToken: cancellationToken);

            using var connection = await _connectionCreator.CreateAsync();
            await connection.ExecuteAsync(command);
        }

        public async Task ChangeProductCategoryAsync(string productName, int newCategoryId, CancellationToken cancellationToken)
        {
            var command = new CommandDefinition(
                "update dbo.Products set CategoryId = @newCategoryId where Name = @productName",
                new
                {
                    productName,
                    newCategoryId,
                },
                cancellationToken: cancellationToken);

            using var connection = await _connectionCreator.CreateAsync();
            await connection.ExecuteAsync(command);
        }

        public async Task ChangeProductCountAsync(string productName, int count, CancellationToken cancellationToken)
        {
            var command = new CommandDefinition(
                "update dbo.Products set Count = @count where Name = @productName",
                new
                {
                    productName,
                    count,
                },
                cancellationToken: cancellationToken);

            using var connection = await _connectionCreator.CreateAsync();
            await connection.ExecuteAsync(command);
        }

        public async Task DeleteProductAsync(string productName, CancellationToken cancellationToken)
        {
            var command = new CommandDefinition(
                "delete from dbo.Products where Name = @productName",
                new
                {
                    productName,
                },
                cancellationToken: cancellationToken);

            using var connection = await _connectionCreator.CreateAsync();
            await connection.ExecuteAsync(command);
        }

        public async Task<bool> ExistsAsync(string productName, CancellationToken cancellationToken)
        {
            var command = new CommandDefinition(
                "select count(1) from dbo.Products where Name = @productName",
                new
                {
                    productName,
                },
                cancellationToken: cancellationToken);

            using var connection = await _connectionCreator.CreateAsync();
            return await connection.QuerySingleAsync<int>(command) > 0;
        }

        public async IAsyncEnumerable<ProductWithCategoryName> GetAllProductsAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var command = new CommandDefinition(
                """
                select p.Name, c.Name as CategoryName, p.Count from dbo.Products p
                join dbo.Categories c on p.CategoryId = c.Id
                """,
                cancellationToken: cancellationToken);

            using var connection = await _connectionCreator.CreateAsync();
            var reader = (DbDataReader)await connection.ExecuteReaderAsync(command);

            var rowParser = reader.GetRowParser<ProductWithCategoryName>();

            while (await reader.ReadAsync(cancellationToken))
            {
                yield return rowParser(reader);
            }
        }
    }
}