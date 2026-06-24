using Dapper;
using Model;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;

namespace Tests.Products
{
    [TestFixture]
    public class PUT_ChangeProductCount : ProductBaseTestClass
    {
        [Test]
        public async Task ChangeProductCount()
        {
            // Arrange
            await connection.ExecuteAsync("insert into dbo.Categories (Name) values ('Category1')");
            await connection.ExecuteAsync("insert into dbo.Products (Name, Count, CategoryId) values ('Product1', 10, 1)");

            // Act
            var response = await client.PutAsJsonAsync("/products/change-count", new ChangeProductCountDto { Name = "Product1", Count = 20 });

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var product = await connection.QueryFirstOrDefaultAsync<Product>("select CategoryId, Name, Count from dbo.Products");
            Assert.NotNull(product);
            Assert.AreEqual("Product1", product.Name);
            Assert.AreEqual(20, product.Count);
            Assert.AreEqual(1, product.CategoryId);
        }
    }
}
