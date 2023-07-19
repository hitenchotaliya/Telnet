using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MVCTelent.Models;

public partial class TelentDbContext : DbContext
{
    public TelentDbContext()
    {
    }

    public TelentDbContext(DbContextOptions<TelentDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<ImageGellery> ImageGelleries { get; set; }

    public virtual DbSet<ImageGelleryPic> ImageGelleryPics { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<TelentApply> TelentApplies { get; set; }

    public virtual DbSet<TelentFeedback> TelentFeedbacks { get; set; }

    public virtual DbSet<TelentRequest> TelentRequests { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    public virtual DbSet<UserProfileDetail> UserProfileDetails { get; set; }

    public virtual DbSet<VideoGellery> VideoGelleries { get; set; }

    public virtual DbSet<VideoGelleryVideo> VideoGelleryVideos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-530F244;Database=TelentDB;Trusted_Connection=True;TrustServerCertificate=True;Integrated Security=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.ToTable("Admin");

            entity.Property(e => e.AdminId).HasColumnName("admin_id");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Uname).HasColumnName("uname");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CatImg).HasColumnName("cat_img");
            entity.Property(e => e.CatName).HasColumnName("cat_name");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Cid);

            entity.ToTable("City");

            entity.Property(e => e.Name).HasMaxLength(150);

            entity.HasOne(d => d.SidNavigation).WithMany(p => p.Cities)
                .HasForeignKey(d => d.Sid)
                .HasConstraintName("FK_City_State");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.City).HasColumnName("city");
            entity.Property(e => e.ContactNo)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("contact_no");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Img).HasColumnName("img");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Password)
                .HasColumnType("text")
                .HasColumnName("password");
            entity.Property(e => e.State).HasColumnName("state");
        });

        modelBuilder.Entity<ImageGellery>(entity =>
        {
            entity.ToTable("ImageGellery");

            entity.Property(e => e.ImageGelleryId).HasColumnName("image_gellery_id");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.GelleryName).HasColumnName("gellery_name");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.ImageGelleries)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_ImageGellery_Users");
        });

        modelBuilder.Entity<ImageGelleryPic>(entity =>
        {
            entity.Property(e => e.ImageGelleryPicId).HasColumnName("image_gellery_pic_id");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.ImageGelleryId).HasColumnName("image_gellery_id");
            entity.Property(e => e.PicName).HasColumnName("pic_name");

            entity.HasOne(d => d.ImageGellery).WithMany(p => p.ImageGelleryPics)
                .HasForeignKey(d => d.ImageGelleryId)
                .HasConstraintName("FK_ImageGelleryPics_ImageGellery");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.Sid);

            entity.ToTable("State");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<TelentApply>(entity =>
        {
            entity.ToTable("TelentApply");

            entity.Property(e => e.TelentApplyId).HasColumnName("telent_apply_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.Message)
                .HasColumnType("text")
                .HasColumnName("message");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TelentReqId).HasColumnName("telent_req_id");

            entity.HasOne(d => d.Category).WithMany(p => p.TelentApplies)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_TelentApply_Category");

            entity.HasOne(d => d.TelentReq).WithMany(p => p.TelentApplies)
                .HasForeignKey(d => d.TelentReqId)
                .HasConstraintName("FK_TelentApply_TelentRequest");
        });

        modelBuilder.Entity<TelentFeedback>(entity =>
        {
            entity.ToTable("TelentFeedback");

            entity.Property(e => e.TelentFeedbackId).HasColumnName("telent_feedback_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Rating)
                .HasColumnType("text")
                .HasColumnName("rating");
            entity.Property(e => e.Review)
                .HasColumnType("text")
                .HasColumnName("review");

            entity.HasOne(d => d.Category).WithMany(p => p.TelentFeedbacks)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_TelentFeedback_Category");

            entity.HasOne(d => d.Customer).WithMany(p => p.TelentFeedbacks)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_TelentFeedback_Customers");
        });

        modelBuilder.Entity<TelentRequest>(entity =>
        {
            entity.HasKey(e => e.TelentReqId);

            entity.ToTable("TelentRequest");

            entity.Property(e => e.TelentReqId).HasColumnName("telent_req_id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.ContactPersonName).HasColumnName("contact_person_name");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.FromDate)
                .HasColumnType("datetime")
                .HasColumnName("from_date");
            entity.Property(e => e.NoOfPerson)
                .HasColumnType("text")
                .HasColumnName("no_of_person");
            entity.Property(e => e.ToDate)
                .HasColumnType("datetime")
                .HasColumnName("to_date");

            entity.HasOne(d => d.Category).WithMany(p => p.TelentRequests)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_TelentRequest_Category");

            entity.HasOne(d => d.Customer).WithMany(p => p.TelentRequests)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_TelentRequest_Customers");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.City).HasColumnName("city");
            entity.Property(e => e.ContactNo)
                .HasMaxLength(10)
                .HasColumnName("contactNo");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("dob");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Fname).HasColumnName("fname");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender");
            entity.Property(e => e.Img).HasColumnName("img");
            entity.Property(e => e.IsPaid).HasColumnName("is_paid");
            entity.Property(e => e.Lname).HasColumnName("lname");
            entity.Property(e => e.PaidDate)
                .HasColumnType("datetime")
                .HasColumnName("paid_date");
            entity.Property(e => e.Password)
                .HasColumnType("text")
                .HasColumnName("password");
            entity.Property(e => e.State).HasColumnName("state");
            entity.Property(e => e.TrialEndDate)
                .HasColumnType("datetime")
                .HasColumnName("trial_end_date");

            entity.HasOne(d => d.Category).WithMany(p => p.Users)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Users_Category");
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.ToTable("UserProfile");

            entity.Property(e => e.UserProfileId).HasColumnName("user_profile_id");
            entity.Property(e => e.ImageGelleryId).HasColumnName("image_gellery_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.VideoGelleryId).HasColumnName("video_gellery_id");

            entity.HasOne(d => d.ImageGellery).WithMany(p => p.UserProfiles)
                .HasForeignKey(d => d.ImageGelleryId)
                .HasConstraintName("FK_UserProfile_ImageGellery");

            entity.HasOne(d => d.User).WithMany(p => p.UserProfiles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserProfile_Users");
        });

        modelBuilder.Entity<UserProfileDetail>(entity =>
        {
            entity.ToTable("UserProfileDetail");

            entity.Property(e => e.UserProfileDetailId).HasColumnName("user_profile_detail_id");
            entity.Property(e => e.Certificate)
                .HasColumnType("text")
                .HasColumnName("certificate");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Education)
                .HasColumnType("text")
                .HasColumnName("education");
            entity.Property(e => e.Experience)
                .HasColumnType("text")
                .HasColumnName("experience");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.UserProfileDetails)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserProfileDetail_Users");
        });

        modelBuilder.Entity<VideoGellery>(entity =>
        {
            entity.ToTable("VideoGellery");

            entity.Property(e => e.VideoGelleryId).HasColumnName("video_gellery_id");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.GelleryName).HasColumnName("gellery_name");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.VideoGelleries)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_VideoGellery_Users");
        });

        modelBuilder.Entity<VideoGelleryVideo>(entity =>
        {
            entity.Property(e => e.VideoGelleryVideoId).HasColumnName("video_gellery_video_id");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.VideoGelleryId).HasColumnName("video_gellery_id");
            entity.Property(e => e.VideoLink)
                .HasColumnType("text")
                .HasColumnName("video_link");

            entity.HasOne(d => d.VideoGellery).WithMany(p => p.VideoGelleryVideos)
                .HasForeignKey(d => d.VideoGelleryId)
                .HasConstraintName("FK_VideoGelleryVideos_VideoGellery");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
