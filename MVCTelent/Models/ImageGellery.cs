using System;
using System.Collections.Generic;

namespace MVCTelent.Models;

public partial class ImageGellery
{
    public int ImageGelleryId { get; set; }

    public string? GelleryName { get; set; }

    public int? UserId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<ImageGelleryPic> ImageGelleryPics { get; } = new List<ImageGelleryPic>();

    public virtual User? User { get; set; }

    public virtual ICollection<UserProfile> UserProfiles { get; } = new List<UserProfile>();
}
