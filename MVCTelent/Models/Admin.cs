using System;
using System.Collections.Generic;

namespace MVCTelent.Models;

public partial class Admin
{
    public int AdminId { get; set; }

    public string? Name { get; set; }

    public string? Uname { get; set; }

    public string? Password { get; set; }

    public bool? Isactive { get; set; }
}
