using System;
using System.Collections.Generic;

namespace Lab2proj.Models;

public partial class EmployeeMonthlyShift
{
    public string FullName { get; set; } = null!;

    public int? MonthlyShiftCount { get; set; }
}
