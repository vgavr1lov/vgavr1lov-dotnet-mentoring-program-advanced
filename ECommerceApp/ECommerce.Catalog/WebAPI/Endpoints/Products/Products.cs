using ECommerce.Catalog.Application.Products.Commands.AddProduct;
using ECommerce.Catalog.Application.Products.Commands.DeleteProduct;
using ECommerce.Catalog.Application.Products.Commands.UpdateProduct;
using ECommerce.Catalog.Application.Products.Contracts;
using ECommerce.Catalog.Application.Products.Queries.GetProduct;
using ECommerce.Catalog.Application.Products.Queries.GetProducts;
using ECommerce.Catalog.WebAPI.Contracts;
using ECommerce.Catalog.WebAPI.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Catalog.WebAPI.Endpoints.Products;

public class Products
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet("/{ProductId:long}", GetProduct)
            .WithName("GetProduct");
        groupBuilder.MapGet("/", GetAllProducts)
            .WithName("GetAllProducts");
        groupBuilder.MapPost("/", AddProduct)
            .WithName("AddProduct"); ;
        groupBuilder.MapPut("/", UpdateProduct)
            .WithName("UpdateProduct"); ;
        groupBuilder.MapDelete("/{ProductId:long}", DeleteProduct)
            .WithName("DeleteProduct"); ;
    }

    /// <summary>
    /// Gets Product data.
    /// </summary>
    /// <param name="productId">The Product Id.</param>
    /// <returns>The Product data</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /products/1
    ///
    /// </remarks>
    /// <response code="200">Returns the Product data</response>
    /// <response code="404">Product not found</response>
    [EndpointSummary("Get a Product")]
    [EndpointDescription("Gets a Product and returns a Product.")]
    public static async Task<IResult> GetProduct(
        long productId,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetProductQuery(productId);
        var response = await sender.Send(query, cancellationToken);

        return Results.Ok(response);
    }

    [EndpointSummary("Get Products")]
    [EndpointDescription("Get all Products and returns a list of all Products. (filtration by category id and pagination)")]
    public static async Task<IResult> GetAllProducts(
    [FromServices] ISender sender,
    CancellationToken cancellationToken,
    [FromQuery] int pageNumber = 0,
    [FromQuery] int pageSize = 10,
    [FromQuery] long? categoryId = null)
    {
        var request = new GetProductsRequest { PageNumber = pageNumber, PageSize = pageSize, CategoryId = categoryId };
        var query = new GetProductsQuery(request);
        var response = await sender.Send(query, cancellationToken);

        return Results.Ok(response);
    }

    /// <summary>
    /// Creates a new Product.
    /// </summary>
    /// <param name="request">The Product data to create.</param>
    /// <returns>The ID of the created Product</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /products
    ///     {
    ///        "id": 1,
    ///        "name": "Laptop",
    ///        "description": "Laptop",
    ///        "imageUrl": "https://example.com/laptop.jpg",
    ///        "imageAltText": "Laptop",
    ///        "categoryId": 1,
    ///        "amount": 120,
    ///        "currency": "PLN",
    ///        "quantity": 1
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Returns the ID of the created Product</response>
    /// <response code="409">Product already exists</response>
    [EndpointSummary("Create a new Product")]
    [EndpointDescription("Creates a new Product and returns the ID of the created Product.")]
    public static async Task<IResult> AddProduct(
        [FromBody] CreateProductRequest request,
        [FromServices] ISender sender,
        [FromServices] ILinkService linkService,
        CancellationToken cancellationToken)
    {
        var command = new AddProductCommand(request);
        var productId = await sender.Send(command, cancellationToken);

        var resource = new Resource<long>
        {
            Data = productId
        };

        resource.Links.Add(new LinkResource
        {
            Href = linkService.GenerateLink("GetProduct", new { productId }),
            Rel = "self",
            Method = "GET"
        });
        resource.Links.Add(new LinkResource
        {
            Href = linkService.GenerateLink("UpdateProduct", null),
            Rel = "update",
            Method = "PUT"
        });
        resource.Links.Add(new LinkResource
        {
            Href = linkService.GenerateLink("DeleteProduct", new { productId }),
            Rel = "delete",
            Method = "DELETE"
        });

        return Results.Created($"/products/{productId}", resource);
    }

    /// <summary>
    /// Updates Product data.
    /// </summary>
    /// <param name="request">The Product data.</param>
    /// <returns>Ok</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /products
    ///     {
    ///        "id": 1,
    ///        "name": "Laptop - updated",
    ///        "description": "Laptop - updated",
    ///        "imageUrl": "https://example.com/laptop.jpg",
    ///        "imageAltText": "Laptop - updated",
    ///        "categoryId": 1,
    ///        "amount": 120,
    ///        "currency": "PLN",
    ///        "quantity": 1
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Ok</response>
    /// <response code="404">Product not found</response>
    [EndpointSummary("Update a Product")]
    [EndpointDescription("Updates a Product and returns Ok.")]
    public static async Task<IResult> UpdateProduct(
    [FromBody] UpdateProductRequest request,
    [FromServices] ISender sender,
    CancellationToken cancellationToken)
    {
        var command = new UpdateProductCommand(request);
        await sender.Send(command, cancellationToken);

        return Results.Ok();
    }

    /// <summary>
    /// Deletes Product data.
    /// </summary>
    /// <param name="productId">The Product Id.</param>
    /// <returns>Ok</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /products/1
    ///
    /// </remarks>
    /// <response code="200">Ok</response>
    /// <response code="404">Product not found</response>
    [EndpointSummary("Delete a Product")]
    [EndpointDescription("Gets a Product and returns a Product.")]
    public static async Task<IResult> DeleteProduct(
    long productId,
    [FromServices] ISender sender,
    CancellationToken cancellationToken)
    {
        var command = new DeleteProductCommand(productId);
        await sender.Send(command, cancellationToken);

        return Results.Ok();
    }
}
