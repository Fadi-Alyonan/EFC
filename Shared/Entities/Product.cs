using System;
using System.Collections.Generic;

namespace Shared.Entities;

public partial class Product
{
    public Guid ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string ProductDescription { get; set; } = null!;

    public int QuantityInStock { get; set; }

    public int? CategoryId { get; set; }

    public int? ManufacturerId { get; set; }

    public int? ProductionId { get; set; }

    public int? PriceId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Manufacturer? Manufacturer { get; set; }

    public virtual Price? Price { get; set; }

    public virtual ProductionInformation? Production { get; set; }
}
