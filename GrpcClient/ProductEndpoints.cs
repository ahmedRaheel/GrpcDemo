using Grpc.Net.Client;
using GrpcDemo;
using Google.Protobuf.WellKnownTypes;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var productGroup = app.MapGroup("/Product").WithOpenApi();

        // Endpoint to get all products
        productGroup.MapGet("/", async (ProductService.ProductServiceClient client, CancellationToken cancellationToken) =>
        {           
            var request = new Empty();
            var response = await client.GetProductsAsync(request, cancellationToken: cancellationToken);
            return response;
        });

        // Endpoint to get a product by ID
        productGroup.MapGet("/{id:int}", async (int id, ProductService.ProductServiceClient client) =>
        {           
            var request = new GetProductByIdRequest { Id = id };
            var response = client.GetProductById(request);
            return response;
        });

        // Endpoint to create a new product
        productGroup.MapPost("/", async (CreateProductRequest request, ProductService.ProductServiceClient client, CancellationToken cancellationToken) =>
        {           
            var response = await client.CreateProductAsync(request, cancellationToken: cancellationToken);
            return response;
        });

        return app;
    }
}
