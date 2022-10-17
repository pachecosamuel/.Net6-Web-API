public class Product
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }

    public string? Description { get; set; }

    public Category Category { get; set; }

    public List<Tag> Tags { get; set; }

    public override string? ToString()
    {
        return $"{ProductId} & {ProductName}";
    }
}
