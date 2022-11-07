namespace IWantApp.Endpoints.Products;

public record ProductResponse(string Name, string CategoryName, string ProductDescription, decimal Price, Guid Id);
