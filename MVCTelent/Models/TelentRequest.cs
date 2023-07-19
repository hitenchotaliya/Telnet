using System;
using System.Collections.Generic;

namespace MVCTelent.Models;

public partial class TelentRequest
{
    public int TelentReqId { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public int? CategoryId { get; set; }

    public int? CustomerId { get; set; }

    public string? NoOfPerson { get; set; }

    public string? Address { get; set; }

    public string? Description { get; set; }

    public long? Amount { get; set; }

    public string? ContactPersonName { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<TelentApply> TelentApplies { get; } = new List<TelentApply>();
}
