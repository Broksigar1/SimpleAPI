using Dapper;
using Model;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;

namespace Tests.Products
{
    [TestFixture]
    public class POST_AddProduct : ProductBaseTestClass
    {
        [Test]
        public async Task AddNewProduct()
        {
            // Arrange
            await connection.ExecuteAsync("insert into dbo.Categories (Name) values ('Category1')");

            // Act
            var product = new Product(1, "Product1", 10);
            var response = await client.PostAsJsonAsync("/products/add", product);

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

            product = await connection.QueryFirstOrDefaultAsync<Product>("select CategoryId, Name, Count from dbo.Products");
            Assert.NotNull(product);
            Assert.AreEqual("Product1", product.Name);
            Assert.AreEqual(10, product.Count);
            Assert.AreEqual(1, product.CategoryId);
        }

        [Test]
        public async Task AddProductWithExistingName()
        {
            // Arrange
            await connection.ExecuteAsync("insert into dbo.Categories (Name) values ('Category1')");
            await connection.ExecuteAsync("insert into dbo.Products (Name, Count, CategoryId) values ('Product1', 10, 1)");

            // Act
            var product = new Product(1, "Product1", 20);
            var response = await client.PostAsJsonAsync("/products/add", product);

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task AddProductWithNotExistingCategoryId()
        {
            // Act
            var product = new Product(1, "Product1", 10);
            var response = await client.PostAsJsonAsync("/products/add", product);
            
            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }
    }
}
