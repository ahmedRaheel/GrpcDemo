using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using static GrpcDemo.ProductService;

namespace GrpcDemo.Services
{
    public class ProductService : ProductServiceBase
    {
        private List<Product> _products = new List<Product>
        {
            new Product {  Id = 1, Name = "Test Product", Amount = 10, Description = "Testing "}
        };
        public ProductService() 
        {
        }

        public override Task<Product> CreateProduct(CreateProductRequest request, ServerCallContext context)
        {
            var newProduct = new Product
            {
                Id = _products.Count + 1, 
                Name = request.Name, 
                Description = request.Description, 
                Amount = request.Amount
            };
            _products.Add(newProduct);
            return Task.FromResult(newProduct);
        }
        public override Task<Product> GetProductById(GetProductByIdRequest request, ServerCallContext context)
        {
            var product = _products.FirstOrDefault(x => x.Id == request.Id);
            if (product is null) 
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"No product with Id {request.Id} found"));
            }

            return Task.FromResult(product);
        }


        public override Task<ProductList> GetProducts(Empty request, ServerCallContext context)
        {
            var response = new ProductList();
            if (_products is null || _products.Count == 0) 
            {
                throw new RpcException(new Status(StatusCode.NotFound, "No products are registered"));
            }
            response.Products.AddRange(_products);

            return Task.FromResult(response);
        }
    }
}
