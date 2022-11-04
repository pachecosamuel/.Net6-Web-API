public static class ProductRepository
{
    public static List<Product> ProductList { get; set; } = new();

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
        return ProductList.FirstOrDefault(x => x.ProductId == id);
    }
}
