using System;
using System.Collections.Generic;

namespace MVCTelent.Models;

public partial class City
{
    public int Cid { get; set; }

    public string? Name { get; set; }

    public int? Sid { get; set; }

    public virtual State? SidNavigation { get; set; }
}
