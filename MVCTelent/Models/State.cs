using System;
using System.Collections.Generic;

namespace MVCTelent.Models;

public partial class State
{
    public int Sid { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<City> Cities { get; } = new List<City>();
}
