namespace IWantApp.Endpoints.Products;

public record ProductRequest(string Name, Guid CategoryId, string ProductDescription, bool HasStock, bool Active);
