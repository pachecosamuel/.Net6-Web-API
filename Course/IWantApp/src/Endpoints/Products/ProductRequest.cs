namespace IWantApp.Endpoints.Products;

public record ProductRequest(string Name, Guid CategoryId, string ProductDescription, bool HasStock, decimal Price, bool Active);
