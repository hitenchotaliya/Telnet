using System;
using System.Collections.Generic;

namespace MVCTelent.Models;

public partial class UserProfile
{
    public int UserProfileId { get; set; }

    public int? UserId { get; set; }

    public int? ImageGelleryId { get; set; }

    public int? VideoGelleryId { get; set; }

    public virtual ImageGellery? ImageGellery { get; set; }

    public virtual User? User { get; set; }

    public virtual VideoGellery? VideoGellery { get; set; }
}
