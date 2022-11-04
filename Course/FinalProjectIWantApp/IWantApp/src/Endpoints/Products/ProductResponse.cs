namespace IWantApp.Endpoints.Products;

public record ProductResponse(string Name, string CategoryName, string ProductDescription, bool HasStock, bool Active, decimal Price, Guid Id);
