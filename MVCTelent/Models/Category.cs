using System;
using System.Collections.Generic;

namespace MVCTelent.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? CatName { get; set; }

    public string? CatImg { get; set; }

    public bool? Isactive { get; set; }

    public virtual ICollection<TelentApply> TelentApplies { get; } = new List<TelentApply>();

    public virtual ICollection<TelentFeedback> TelentFeedbacks { get; } = new List<TelentFeedback>();

    public virtual ICollection<TelentRequest> TelentRequests { get; } = new List<TelentRequest>();

    public virtual ICollection<User> Users { get; } = new List<User>();
}
