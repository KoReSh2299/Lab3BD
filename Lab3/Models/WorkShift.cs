using System;
using System.Collections.Generic;

namespace Lab2proj.Models;

public partial class WorkShift
{
    public int Id { get; set; }

    public DateTime ShiftStartTime { get; set; }

    public DateTime ShiftEndTime { get; set; }

    public int EmployeeId { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual ICollection<ParkingRecordsWorkShift> ParkingRecordsWorkShifts { get; set; } = new List<ParkingRecordsWorkShift>();
}
