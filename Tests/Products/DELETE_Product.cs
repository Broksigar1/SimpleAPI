using API;
using Dapper;
using DatabaseService;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Testing.Platform.Services;
using Model;
using NUnit.Framework;
using System.Data;
using System.Net;

namespace Tests.Products
{
    internal class DELETE_Product
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;
        private IDbConnection _connection;

        [SetUp]
        public async Task Setup()
        {
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
            _connection = await _factory.Services.GetRequiredService<DbConnectionCreator>().CreateAsync();
            await _connection.ExecuteAsync("TRUNCATE TABLE dbo.Products");
            await _connection.ExecuteAsync("TRUNCATE TABLE dbo.Categories");
        }

        [Test]
        public async Task DeleteProduct()
        {
            // Arrange
            await _connection.ExecuteAsync("insert into dbo.Categories (Name) values ('Category1')");
            await _connection.ExecuteAsync("insert into dbo.Products (Name, Count, CategoryId) values ('Product1', 10, 1)");

            // Act
            var response = await _client.DeleteAsync("/products/delete/Product1");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var product = await _connection.QueryFirstOrDefaultAsync<Product>("select CategoryId, Name, Count from dbo.Products");
            Assert.Null(product);
        }

        [TearDown]
        public async Task TearDown()
        {
            _factory.Dispose();
            _client.Dispose();
            _connection.Dispose();
        }
    }
}
