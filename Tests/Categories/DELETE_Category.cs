using Dapper;
using NUnit.Framework;
using System.Net;

namespace Tests.Categories
{
    [TestFixture]
    public class DELETE_Category : CategoryBaseTestClass
    {
        [Test]
        public async Task DeleteCategory()
        {
            // Arrange
            await connection.ExecuteAsync("insert into dbo.Categories (Name) values ('Category1')");

            // Act
            var response = await client.DeleteAsync("/categories/delete/Category1");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var name = await connection.QueryFirstOrDefaultAsync<string>("select Name from dbo.Categories");
            Assert.Null(name);
        }
    }
}
