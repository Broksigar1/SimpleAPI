using Dapper;
using Model;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;

namespace Tests.Categories
{
    [TestFixture]
    public class PUT_RenameCategory : CategoryBaseTestClass
    {
        [Test]
        public async Task RenameExistingCategory()
        {
            // Arrange
            await connection.ExecuteAsync("insert into dbo.Categories (Name) values ('Category1')");

            // Act
            var response = await client.PutAsJsonAsync("/categories/rename", new RenameCategoryDto { Name = "Category1", NewName = "Category2" });

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var name = await connection.QueryFirstOrDefaultAsync<string>("select Name from dbo.Categories");
            Assert.NotNull(name);
            Assert.AreEqual("Category2", name);
        }

        [Test]
        public async Task RenameNonExistingCategory()
        {
            // Act
            var response = await client.PutAsJsonAsync("/categories/rename", new RenameCategoryDto { Name = "Unknown", NewName = "Category2" });

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
