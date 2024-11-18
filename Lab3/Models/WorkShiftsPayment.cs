using System;
using System.Collections.Generic;

namespace Lab3.Models;

public partial class WorkShiftsPayment
{
    public int Id { get; set; }

    public int WorkShiftId { get; set; }

    public int PaymentId { get; set; }

    public virtual Payment Payment { get; set; } = null!;

    public virtual WorkShift WorkShift { get; set; } = null!;
}
