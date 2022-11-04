public class Product
{
    public string ProductName { get; set; }

    public int Id { get; set; }

    public string ProductDescription { get; set; }

    public Category Category { get; set; }

    public List<Tag> Tags { get; set; } = new();

    public override string? ToString()
    {
        return $"{ProductName} & {Id}";
    }
}
