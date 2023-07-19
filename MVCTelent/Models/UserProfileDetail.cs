using System;
using System.Collections.Generic;

namespace MVCTelent.Models;

public partial class UserProfileDetail
{
    public int UserProfileDetailId { get; set; }

    public int? UserId { get; set; }

    public string? Education { get; set; }

    public string? Certificate { get; set; }

    public string? Experience { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual User? User { get; set; }
}
