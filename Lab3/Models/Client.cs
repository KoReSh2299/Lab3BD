using System;
using System.Collections.Generic;

namespace Lab3.Models;

public partial class Client
{
    public int Id { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public string Telephone { get; set; } = null!;

    public bool IsRegularClient { get; set; }

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
