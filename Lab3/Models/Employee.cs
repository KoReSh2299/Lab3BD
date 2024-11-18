using System;
using System.Collections.Generic;

namespace Lab3.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public virtual ICollection<WorkShift> WorkShifts { get; set; } = new List<WorkShift>();
}
