using Dapper;
using NUnit.Framework;
using System.Net;

namespace Tests.Categories
{
    [TestFixture]
    public class POST_AddCategory : CategoryBaseTestClass
    {
        [Test]
        public async Task AddNewCategory()
        {
            // Act
            var response = await client.PostAsync("/categories/add/Category1", null);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var name = await connection.QueryFirstOrDefaultAsync<string>("select Name from dbo.Categories");
            Assert.NotNull(name);
            Assert.AreEqual("Category1", name);
        }

        [Test]
        public async Task AddExistingCategory()
        {
            // Arrange
            await connection.ExecuteAsync("insert into dbo.Categories (Name) values ('Category1')");

            // Act
            var response = await client.PostAsync("/categories/add/Category1", null);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
