using ECommerce.Catalog.Application.Categories.Commands.AddCategory;
using ECommerce.Catalog.Application.Categories.Commands.DeleteCategory;
using ECommerce.Catalog.Application.Categories.Commands.UpdateCategory;
using ECommerce.Catalog.Application.Categories.Contracts;
using ECommerce.Catalog.Application.Categories.Queries.GetAllCategories;
using ECommerce.Catalog.Application.Categories.Queries.GetCategory;
using ECommerce.Catalog.WebAPI.Contracts;
using ECommerce.Catalog.WebAPI.Infrastructure;
using ECommerce.Catalog.WebAPI.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Catalog.WebAPI.Endpoints.Categories;

public class Categories : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet("/{categoryId:long}", GetCategory)
            .WithName("GetCategory");
        groupBuilder.MapGet("/", GetAllCategories)
            .WithName("GetAllCategories");
        groupBuilder.MapPost("/", AddCategory)
            .WithName("AddCategory");
        groupBuilder.MapPut("/", UpdateCategory)
            .WithName("UpdateCategory");
        groupBuilder.MapDelete("/{categoryId:long}", DeleteCategory)
            .WithName("DeleteCategory");
    }

    /// <summary>
    /// Gets Category data.
    /// </summary>
    /// <param name="categoryId">The Category Id.</param>
    /// <returns>The Category data</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /categories/1
    ///
    /// </remarks>
    /// <response code="200">Returns the Category data</response>
    /// <response code="404">Category not found</response>
    [EndpointSummary("Get a Category")]
    [EndpointDescription("Gets a Category and returns a Category.")]
    public static async Task<IResult> GetCategory(
        long categoryId,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var response = await sender.Send(new GetCategoryQuery(categoryId), cancellationToken);

        return Results.Ok(response);
    }

    [EndpointSummary("Get All Categories")]
    [EndpointDescription("Get all Categories and returns a list of all Categories.")]
    public static async Task<IResult> GetAllCategories(
    [FromServices] ISender sender,
    CancellationToken cancellationToken)
    {
        var response = await sender.Send(new GetAllCategoriesQuery(), cancellationToken);

        return Results.Ok(response);
    }

    /// <summary>
    /// Creates a new Category.
    /// </summary>
    /// <param name="request">The Category data to create.</param>
    /// <returns>The ID of the created Category</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /categories
    ///     {
    ///        "id": 1,
    ///        "name": "Electronics",
    ///        "imageUrl": "https://example.com/category.jpg",
    ///        "imageAltText": "Electronic devices and gadgets",
    ///        "parentCategoryId": null
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Returns the ID of the created Category</response>
    /// <response code="409">Category already exists</response>
    public static async Task<IResult> AddCategory(
        [FromBody] CreateCategoryRequest request,
        [FromServices] ISender sender,
        [FromServices] ILinkService linkService,
        CancellationToken cancellationToken)
    {
        var command = new AddCategoryCommand(request);
        var categoryId = await sender.Send(command, cancellationToken);

        var resource = new Resource<long>
        {
            Data = categoryId
        };

        resource.Links.Add(new LinkResource
        {
            Href = linkService.GenerateLink("GetCategory", new { categoryId }),
            Rel = "self",
            Method = "GET"
        });
        resource.Links.Add(new LinkResource
        {
            Href = linkService.GenerateLink("UpdateCategory", null),
            Rel = "update",
            Method = "PUT"
        });
        resource.Links.Add(new LinkResource
        {
            Href = linkService.GenerateLink("DeleteCategory", new { categoryId }),
            Rel = "delete",
            Method = "DELETE"
        });

        return Results.Created($"/categories/{categoryId}", resource);
    }

    /// <summary>
    /// Updates Category data.
    /// </summary>
    /// <param name="request">The Category data.</param>
    /// <returns>Ok</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /categories
    ///     {
    ///        "id": 1,
    ///        "name": "Electronics - updated",
    ///        "imageUrl": "https://example.com/category.jpg",
    ///        "imageAltText": "Electronic devices and gadgets",
    ///        "parentCategoryId": null
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Ok</response>
    /// <response code="404">Category not found</response>
    [EndpointSummary("Update a Category")]
    [EndpointDescription("Updates a Category and returns Ok.")]
    public static async Task<IResult> UpdateCategory(
    [FromBody] UpdateCategoryRequest request,
    [FromServices] ISender sender,
    CancellationToken cancellationToken)
    {
        var command = new UpdateCategoryCommand(request);
        await sender.Send(command, cancellationToken);

        return Results.Ok();
    }

    /// <summary>
    /// Deletes Category data.
    /// </summary>
    /// <param name="categoryId">The Category Id.</param>
    /// <returns>Ok</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /categories/1
    ///
    /// </remarks>
    /// <response code="200">Ok</response>
    /// <response code="404">Category not found</response>
    [EndpointSummary("Delete a Category")]
    [EndpointDescription("Delete a Category and returns Ok.")]
    public static async Task<IResult> DeleteCategory(
    long categoryId,
    [FromServices] ISender sender,
    CancellationToken cancellationToken)
    {
        var command = new DeleteCategoryCommand(categoryId);
        await sender.Send(command, cancellationToken);

        return Results.Ok();
    }
}