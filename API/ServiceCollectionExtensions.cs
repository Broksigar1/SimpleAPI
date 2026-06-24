using DatabaseService;
using DatabaseService.Repositories;
using Microsoft.OpenApi;

namespace API
{
    public static class ServiceCollectionExtensions
    {
        public static void Register(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ExceptionHandlingMiddleware>();
            serviceCollection.AddTransient<DbConnectionCreator, SQLServerConnectionCreator>();
            serviceCollection.AddTransient<IProductRepository, ProductRepository>();
            serviceCollection.AddTransient<ICategoryRepository, CategoryRepository>();
            serviceCollection.AddEndpointsApiExplorer();
            serviceCollection.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Simple API",
                    Description = "API for warehouse",
                });
            });
        }
    }
}
