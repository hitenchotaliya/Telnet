using System;
using System.Collections.Generic;

namespace MVCTelent.Models;

public partial class TelentApply
{
    public int TelentApplyId { get; set; }

    public int? TelentReqId { get; set; }

    public int? CategoryId { get; set; }

    public DateTime? Date { get; set; }

    public bool? Status { get; set; }

    public string? Message { get; set; }

    public virtual Category? Category { get; set; }

    public virtual TelentRequest? TelentReq { get; set; }
}
