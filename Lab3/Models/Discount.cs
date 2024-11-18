using System;
using System.Collections.Generic;

namespace Lab3.Models;

public partial class Discount
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public int Percentage { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
