using Dapper;
using Model;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;

namespace Tests.Products
{
    [TestFixture]
    public class GET_products : ProductBaseTestClass
    {
        [Test]
        public async Task GetAllProducts()
        {
            // Arrange
            await connection.ExecuteAsync("insert into dbo.Categories (Name) values ('Category1')");
            await connection.ExecuteAsync("insert into dbo.Products (Name, Count, CategoryId) values ('Product1', 10, 1)");

            // Act
            var response = await client.GetAsync("/products");

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            
            var products = await response.Content.ReadFromJsonAsync<List<ProductWithCategoryName>>();
            Assert.NotNull(products);
            Assert.AreEqual(1, products.Count);
            Assert.AreEqual("Product1", products[0].Name);
            Assert.AreEqual(10, products[0].Count);
            Assert.AreEqual("Category1", products[0].CategoryName);
        }
    }
}
