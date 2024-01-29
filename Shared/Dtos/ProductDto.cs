namespace Shared.Dtos;

public class ProductDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public string ProductDescription { get; set; } = null!;
    public int QuantityInStock { get; set; }
    public DateOnly? ProductionDate { get; set; }
    public string CategoryName { get; set; } = null!;
    public string ManufacturerName { get; set; } = null!;
    public decimal ProductPrice { get; set; }

}
