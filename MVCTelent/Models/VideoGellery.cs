using System;
using System.Collections.Generic;

namespace MVCTelent.Models;

public partial class VideoGellery
{
    public int VideoGelleryId { get; set; }

    public string? GelleryName { get; set; }

    public int? UserId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual User? User { get; set; }

    public virtual ICollection<VideoGelleryVideo> VideoGelleryVideos { get; } = new List<VideoGelleryVideo>();
}
