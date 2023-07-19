using System;
using System.Collections.Generic;

namespace MVCTelent.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? Fname { get; set; }

    public string? Lname { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Gender { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? ContactNo { get; set; }

    public int? CategoryId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? TrialEndDate { get; set; }

    public bool? IsPaid { get; set; }

    public DateTime? PaidDate { get; set; }

    public string? Img { get; set; }

    public DateTime? Dob { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<ImageGellery> ImageGelleries { get; } = new List<ImageGellery>();

    public virtual ICollection<UserProfileDetail> UserProfileDetails { get; } = new List<UserProfileDetail>();

    public virtual ICollection<UserProfile> UserProfiles { get; } = new List<UserProfile>();

    public virtual ICollection<VideoGellery> VideoGelleries { get; } = new List<VideoGellery>();
}
