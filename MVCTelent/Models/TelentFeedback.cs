using System;
using System.Collections.Generic;

namespace MVCTelent.Models;

public partial class TelentFeedback
{
    public int TelentFeedbackId { get; set; }

    public int? CategoryId { get; set; }

    public string? Review { get; set; }

    public string? Rating { get; set; }

    public int? CustomerId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Customer? Customer { get; set; }
}
