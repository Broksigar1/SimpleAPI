using Dapper;
using Model;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace DatabaseService.Repositories
{
    public class CategoryRepository : Repository, ICategoryRepository
    {
        public CategoryRepository(DbConnectionCreator connectionCreator) : base(connectionCreator)
        {
        }

        public async Task AddCategoryAsync(string name, CancellationToken token)
        {
            var command = new CommandDefinition(
                "insert into dbo.Categories (Name) values (@name)",
                new { name },
                cancellationToken: token);

            using var connection = await _connectionCreator.CreateAsync();
            await connection.ExecuteAsync(command);
        }

        public async Task DeleteCategoryAsync(string name, CancellationToken token)
        {
            var command = new CommandDefinition(
                "delete from dbo.Categories where Name = @name",
                new { name },
                cancellationToken: token);

            using var connection = await _connectionCreator.CreateAsync();
            await connection.ExecuteAsync(command);
        }

        public async Task<bool> ExistsAsync(string name, CancellationToken token)
        {
            var command = new CommandDefinition(
                "select count(1) from dbo.Categories where Name = @name", 
                new { name },
                cancellationToken: token);

            return await IsSqlCountReturnMoreThanZero(command);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken token)
        {
            var command = new CommandDefinition(
                "select count(1) from dbo.Categories where Id = @id",
                new { id },
                cancellationToken: token);

            return await IsSqlCountReturnMoreThanZero(command);
        }

        public async IAsyncEnumerable<string> GetAllCategoriesAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var command = new CommandDefinition(
                "select c.Name from dbo.Categories c",
                cancellationToken: cancellationToken);

            using var connection = await _connectionCreator.CreateAsync();
            await using var reader = (DbDataReader)await connection.ExecuteReaderAsync(command);

            var nameOrdinal = reader.GetOrdinal("Name");

            while (await reader.ReadAsync(cancellationToken))
            {
                yield return reader.GetString(nameOrdinal);
            }
        }

        public async Task RenameCategoryAsync(string name, string newName, CancellationToken token)
        {
            var command = new CommandDefinition(
                "update dbo.Categories set Name = @newName where Name = @name",
                new { newName, name },
                cancellationToken: token);

            using var connection = await _connectionCreator.CreateAsync();
            await connection.ExecuteAsync(command);
        }

        private async Task<bool> IsSqlCountReturnMoreThanZero(CommandDefinition command)
        {
            using var connection = await _connectionCreator.CreateAsync();
            return await connection.QuerySingleAsync<int>(command) > 0;
        }
    }
}
