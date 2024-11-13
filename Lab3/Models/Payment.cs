using System;
using System.Collections.Generic;

namespace Lab2proj.Models;

public partial class Payment
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public int? TariffId { get; set; }

    public int? DiscountId { get; set; }

    public DateTime? PaymentDate { get; set; }

    public virtual Discount? Discount { get; set; }

    public virtual ICollection<ParkingRecord> ParkingRecords { get; set; } = new List<ParkingRecord>();

    public virtual Tariff? Tariff { get; set; }
}
