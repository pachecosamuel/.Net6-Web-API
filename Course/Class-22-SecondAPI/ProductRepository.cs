public static class ProductRepository
{
    public static List<Product> ProductList { get; set; } = new();

    public static void Init(IConfiguration configuration)
    {
        var products = configuration.GetSection("Products").Get<List<Product>>();
        ProductList = products;
    }

    public static void AddProduct(Product product)
    {
        ProductList.Add(product);
    }

    public static void RemoveProduct(Product product)
    {
        ProductList.Remove(product);
    }

    public static Product GetById(int id)
    {
        return ProductList.FirstOrDefault(x => x.Id == id);
    }
}
