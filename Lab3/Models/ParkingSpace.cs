using System;
using System.Collections.Generic;

namespace Lab2proj.Models;

public partial class ParkingSpace
{
    public int Id { get; set; }

    public bool IsPenalty { get; set; }

    public bool IsFree { get; set; }

    public virtual ICollection<ParkingRecord> ParkingRecords { get; set; } = new List<ParkingRecord>();
}
