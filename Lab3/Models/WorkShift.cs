using System;
using System.Collections.Generic;

namespace Lab3.Models;

public partial class WorkShift
{
    public int Id { get; set; }

    public DateTime ShiftStartTime { get; set; }

    public DateTime ShiftEndTime { get; set; }

    public int EmployeeId { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual ICollection<WorkShiftsPayment> WorkShiftsPayments { get; set; } = new List<WorkShiftsPayment>();
}
