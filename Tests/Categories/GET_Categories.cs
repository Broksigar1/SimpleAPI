using Dapper;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;

namespace Tests.Categories
{
    [TestFixture]
    public class GET_categories : CategoryBaseTestClass
    {
        [Test]
        public async Task GetAllCategories()
        {
            // Arrange
            await connection.ExecuteAsync("insert into dbo.Categories (Name) values ('Category1')");

            // Act
            var response = await client.GetAsync("/categories");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var categories = await response.Content.ReadFromJsonAsync<List<string>>();
            Assert.NotNull(categories);
            Assert.AreEqual(1, categories.Count);
            Assert.AreEqual("Category1", categories[0]);
        }
    }
}
