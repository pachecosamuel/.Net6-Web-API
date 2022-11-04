public record ProductRequest(
    string ProductName, string ProductDescription, int CategoryId, List<string> Tags
);