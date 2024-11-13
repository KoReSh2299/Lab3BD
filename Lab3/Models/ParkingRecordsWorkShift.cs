using System;
using System.Collections.Generic;

namespace Lab2proj.Models;

public partial class ParkingRecordsWorkShift
{
    public int Id { get; set; }

    public int WorkShiftId { get; set; }

    public int ParkingRecordId { get; set; }

    public virtual ParkingRecord ParkingRecord { get; set; } = null!;

    public virtual WorkShift WorkShift { get; set; } = null!;
}
