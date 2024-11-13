using System;
using System.Collections.Generic;

namespace Lab2proj.Models;

public partial class ParkingRecord
{
    public int Id { get; set; }

    public DateTime TimeIn { get; set; }

    public DateTime TimeOut { get; set; }

    public int CarId { get; set; }

    public int ParkingSpaceId { get; set; }

    public int PaymentId { get; set; }

    public virtual Car Car { get; set; } = null!;

    public virtual ICollection<ParkingRecordsWorkShift> ParkingRecordsWorkShifts { get; set; } = new List<ParkingRecordsWorkShift>();

    public virtual ParkingSpace ParkingSpace { get; set; } = null!;

    public virtual Payment Payment { get; set; } = null!;
}
