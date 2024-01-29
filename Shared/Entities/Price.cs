using System;
using System.Collections.Generic;

namespace Shared.Entities;

public partial class Price
{
    public int PriceId { get; set; }

    public decimal ProductPrice { get; set; }

    public DateOnly? PriceDate { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
