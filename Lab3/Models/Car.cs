using System;
using System.Collections.Generic;

namespace Lab2proj.Models;

public partial class Car
{
    public int Id { get; set; }

    public string Brand { get; set; } = null!;

    public string Number { get; set; } = null!;

    public int ClientId { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<ParkingRecord> ParkingRecords { get; set; } = new List<ParkingRecord>();
}
