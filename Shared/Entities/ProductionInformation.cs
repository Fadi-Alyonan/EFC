using System;
using System.Collections.Generic;

namespace Shared.Entities;

public partial class ProductionInformation
{
    public int ProductionId { get; set; }

    public DateOnly? ProductionDate { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
