using API;
using Dapper;
using DatabaseService;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Testing.Platform.Services;
using NUnit.Framework;
using System.Data;

namespace Tests.Categories
{
    public class CategoryBaseTestClass
    {
        protected WebApplicationFactory<Program> factory;
        protected HttpClient client;
        protected IDbConnection connection;

        [SetUp]
        protected async Task Setup()
        {
            factory = new WebApplicationFactory<Program>();
            client = factory.CreateClient();
            connection = await factory.Services.GetRequiredService<DbConnectionCreator>().CreateAsync();
            await connection.ExecuteAsync("TRUNCATE TABLE dbo.Categories");
        }

        [TearDown]
        protected async Task TearDown()
        {
            factory.Dispose();
            client.Dispose();
            connection.Dispose();
        }
    }
}
