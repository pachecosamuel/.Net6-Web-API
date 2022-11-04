public record ProductRequest(
    int ProductId, string ProductName, string Description, int CategoryId, List<string> Tags
);
