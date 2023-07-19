using System;
using System.Collections.Generic;

namespace MVCTelent.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? ContactNo { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public bool? Isactive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Img { get; set; }

    public virtual ICollection<TelentFeedback> TelentFeedbacks { get; } = new List<TelentFeedback>();

    public virtual ICollection<TelentRequest> TelentRequests { get; } = new List<TelentRequest>();
}
