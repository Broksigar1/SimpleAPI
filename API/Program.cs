using DatabaseService.Repositories;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace API
{
    public class Program
    {  
        public static void Main()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.Register();
            var app = builder.Build();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            var productGroup = app.MapGroup("/products");

            productGroup.MapGet("/",
                [EndpointSummary("Get all products with categories")]
                [Tags("Products")]
                (IProductRepository repo, CancellationToken token) =>
                {
                    return repo.GetAllProductsAsync(token);
                });

            productGroup.MapPost("/add",
                [EndpointSummary("Add a new product")]
                [EndpointDescription("Product is added if its name does not exist in database")]
                [Tags("Products")]
                async (Product product, IProductRepository productRepo, ICategoryRepository categoryRepo, CancellationToken token) =>
                {
                    await ProductValidator.ValidateAsync(product, productRepo, categoryRepo, token);

                    await productRepo.AddProductAsync(product, token);
                    return Results.Ok();
                });

            productGroup.MapPut("/change-count",
                [EndpointSummary("Change a product count")]
                [Tags("Products")]
                async (ChangeProductCountDto dto, IProductRepository repo, CancellationToken token) =>
                {
                    await ProductValidator.ValidateProductExistence(dto.Name, repo, token);

                    await repo.ChangeProductCountAsync(dto.Name, dto.Count, token);
                    return Results.Ok();
                });

            productGroup.MapDelete("/delete/{productName:required}",
                [EndpointSummary("Delete a product")]
                [Tags("Products")]
                async ([FromRoute] string productName, IProductRepository repo, CancellationToken token) =>
                {
                    await repo.DeleteProductAsync(productName, token);
                    return Results.Ok();
                });

            var categoryGroup = app.MapGroup("/categories");

            categoryGroup.MapGet("/",
                [EndpointSummary("Get all categories")]
                [Tags("Categories")]
                (ICategoryRepository repo, CancellationToken token) =>
                {
                    return repo.GetAllCategoriesAsync(token);
                });

            categoryGroup.MapPost("/add/{categoryName:regex(^(?=.*\\S).+$)}",
                [EndpointSummary("Add a new category")]
                [Tags("Categories")]
                async ([FromRoute] string categoryName, ICategoryRepository repo, CancellationToken token) =>
                {
                    await CategoryValidator.ValidateAsync(categoryName, repo, token);

                    await repo.AddCategoryAsync(categoryName, token);
                    return Results.Ok();
                });

            categoryGroup.MapPut("/rename",
                [EndpointSummary("Rename a category")]
                [Tags("Categories")]
                async (RenameCategoryDto dto, ICategoryRepository repo, CancellationToken token) =>
                {
                    await CategoryValidator.ValidateExistenceAsync(dto.Name, repo, token);

                    await repo.RenameCategoryAsync(dto.Name, dto.NewName, token);
                    return Results.Ok();
                });

            categoryGroup.MapDelete("/delete/{categoryName:required}",
                [EndpointSummary("Delete a category")]
                [Tags("Categories")]
                async ([FromRoute] string categoryName, ICategoryRepository repo, CancellationToken token) =>
                {
                    await repo.DeleteCategoryAsync(categoryName, token);
                    return Results.Ok();
                });

            app.Run();
        }
    }
}
