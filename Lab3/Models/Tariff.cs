using System;
using System.Collections.Generic;

namespace Lab3.Models;

public partial class Tariff
{
    public int Id { get; set; }

    public decimal Rate { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
