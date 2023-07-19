using System;
using System.Collections.Generic;

namespace MVCTelent.Models;

public partial class VideoGelleryVideo
{
    public int VideoGelleryVideoId { get; set; }

    public int? VideoGelleryId { get; set; }

    public string? VideoLink { get; set; }

    public string? Description { get; set; }

    public virtual VideoGellery? VideoGellery { get; set; }
}
