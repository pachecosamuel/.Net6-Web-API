public class Product
{
    public int Id { get; set; }    
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }

    public Category Category { get; set; }

    public List<Tag> Tags { get; set; }

    public override string ToString()
    {
        return $"{Id} & {ProductName}";
    }
}

