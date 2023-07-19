using System;
using System.Collections.Generic;

namespace MVCTelent.Models;

public partial class ImageGelleryPic
{
    public int ImageGelleryPicId { get; set; }

    public string? PicName { get; set; }

    public string? Description { get; set; }

    public int? ImageGelleryId { get; set; }

    public virtual ImageGellery? ImageGellery { get; set; }
}
