using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCTelent.Migrations
{
    /// <inheritdoc />
    public partial class AddActivationCodeToCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    admin_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    uname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isactive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.admin_id);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cat_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cat_img = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isactive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.category_id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    customer_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    contact_no = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    state = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isactive = table.Column<bool>(type: "bit", nullable: true),
                    ActivationCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.customer_id);
                });

            migrationBuilder.CreateTable(
                name: "State",
                columns: table => new
                {
                    Sid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_State", x => x.Sid);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    state = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    contactNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    category_id = table.Column<int>(type: "int", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    trial_end_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    is_paid = table.Column<bool>(type: "bit", nullable: true),
                    paid_date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_Users_Category",
                        column: x => x.category_id,
                        principalTable: "Category",
                        principalColumn: "category_id");
                });

            migrationBuilder.CreateTable(
                name: "TelentFeedback",
                columns: table => new
                {
                    telent_feedback_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    category_id = table.Column<int>(type: "int", nullable: true),
                    review = table.Column<string>(type: "text", nullable: true),
                    rating = table.Column<string>(type: "text", nullable: true),
                    customer_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelentFeedback", x => x.telent_feedback_id);
                    table.ForeignKey(
                        name: "FK_TelentFeedback_Category",
                        column: x => x.category_id,
                        principalTable: "Category",
                        principalColumn: "category_id");
                    table.ForeignKey(
                        name: "FK_TelentFeedback_Customers",
                        column: x => x.customer_id,
                        principalTable: "Customers",
                        principalColumn: "customer_id");
                });

            migrationBuilder.CreateTable(
                name: "TelentRequest",
                columns: table => new
                {
                    telent_req_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    from_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    to_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    category_id = table.Column<int>(type: "int", nullable: true),
                    customer_id = table.Column<int>(type: "int", nullable: true),
                    no_of_person = table.Column<string>(type: "text", nullable: true),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    amount = table.Column<long>(type: "bigint", nullable: true),
                    contact_person_name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelentRequest", x => x.telent_req_id);
                    table.ForeignKey(
                        name: "FK_TelentRequest_Category",
                        column: x => x.category_id,
                        principalTable: "Category",
                        principalColumn: "category_id");
                    table.ForeignKey(
                        name: "FK_TelentRequest_Customers",
                        column: x => x.customer_id,
                        principalTable: "Customers",
                        principalColumn: "customer_id");
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Cid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Sid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Cid);
                    table.ForeignKey(
                        name: "FK_City_State",
                        column: x => x.Sid,
                        principalTable: "State",
                        principalColumn: "Sid");
                });

            migrationBuilder.CreateTable(
                name: "ImageGellery",
                columns: table => new
                {
                    image_gellery_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    gellery_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageGellery", x => x.image_gellery_id);
                    table.ForeignKey(
                        name: "FK_ImageGellery_Users",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "UserProfileDetail",
                columns: table => new
                {
                    user_profile_detail_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    education = table.Column<string>(type: "text", nullable: true),
                    certificate = table.Column<string>(type: "text", nullable: true),
                    experience = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfileDetail", x => x.user_profile_detail_id);
                    table.ForeignKey(
                        name: "FK_UserProfileDetail_Users",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "VideoGellery",
                columns: table => new
                {
                    video_gellery_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    gellery_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoGellery", x => x.video_gellery_id);
                    table.ForeignKey(
                        name: "FK_VideoGellery_Users",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "TelentApply",
                columns: table => new
                {
                    telent_apply_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    telent_req_id = table.Column<int>(type: "int", nullable: true),
                    category_id = table.Column<int>(type: "int", nullable: true),
                    date = table.Column<DateTime>(type: "datetime", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: true),
                    message = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelentApply", x => x.telent_apply_id);
                    table.ForeignKey(
                        name: "FK_TelentApply_Category",
                        column: x => x.category_id,
                        principalTable: "Category",
                        principalColumn: "category_id");
                    table.ForeignKey(
                        name: "FK_TelentApply_TelentRequest",
                        column: x => x.telent_req_id,
                        principalTable: "TelentRequest",
                        principalColumn: "telent_req_id");
                });

            migrationBuilder.CreateTable(
                name: "ImageGelleryPics",
                columns: table => new
                {
                    image_gellery_pic_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pic_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    image_gellery_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageGelleryPics", x => x.image_gellery_pic_id);
                    table.ForeignKey(
                        name: "FK_ImageGelleryPics_ImageGellery",
                        column: x => x.image_gellery_id,
                        principalTable: "ImageGellery",
                        principalColumn: "image_gellery_id");
                });

            migrationBuilder.CreateTable(
                name: "UserProfile",
                columns: table => new
                {
                    user_profile_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    image_gellery_id = table.Column<int>(type: "int", nullable: true),
                    video_gellery_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfile", x => x.user_profile_id);
                    table.ForeignKey(
                        name: "FK_UserProfile_ImageGellery",
                        column: x => x.image_gellery_id,
                        principalTable: "ImageGellery",
                        principalColumn: "image_gellery_id");
                    table.ForeignKey(
                        name: "FK_UserProfile_Users",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_UserProfile_VideoGellery_video_gellery_id",
                        column: x => x.video_gellery_id,
                        principalTable: "VideoGellery",
                        principalColumn: "video_gellery_id");
                });

            migrationBuilder.CreateTable(
                name: "VideoGelleryVideos",
                columns: table => new
                {
                    video_gellery_video_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    video_gellery_id = table.Column<int>(type: "int", nullable: true),
                    video_link = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoGelleryVideos", x => x.video_gellery_video_id);
                    table.ForeignKey(
                        name: "FK_VideoGelleryVideos_VideoGellery",
                        column: x => x.video_gellery_id,
                        principalTable: "VideoGellery",
                        principalColumn: "video_gellery_id");
                });
            migrationBuilder.AddColumn<string>(
    name: "ActivationCode",
    table: "Customers",
    nullable: true);
            migrationBuilder.CreateIndex(
                name: "IX_City_Sid",
                table: "City",
                column: "Sid");

            migrationBuilder.CreateIndex(
                name: "IX_ImageGellery_user_id",
                table: "ImageGellery",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_ImageGelleryPics_image_gellery_id",
                table: "ImageGelleryPics",
                column: "image_gellery_id");

            migrationBuilder.CreateIndex(
                name: "IX_TelentApply_category_id",
                table: "TelentApply",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_TelentApply_telent_req_id",
                table: "TelentApply",
                column: "telent_req_id");

            migrationBuilder.CreateIndex(
                name: "IX_TelentFeedback_category_id",
                table: "TelentFeedback",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_TelentFeedback_customer_id",
                table: "TelentFeedback",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_TelentRequest_category_id",
                table: "TelentRequest",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_TelentRequest_customer_id",
                table: "TelentRequest",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_image_gellery_id",
                table: "UserProfile",
                column: "image_gellery_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_user_id",
                table: "UserProfile",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_video_gellery_id",
                table: "UserProfile",
                column: "video_gellery_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileDetail_user_id",
                table: "UserProfileDetail",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_category_id",
                table: "Users",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_VideoGellery_user_id",
                table: "VideoGellery",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_VideoGelleryVideos_video_gellery_id",
                table: "VideoGelleryVideos",
                column: "video_gellery_id");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "ImageGelleryPics");

            migrationBuilder.DropTable(
                name: "TelentApply");

            migrationBuilder.DropTable(
                name: "TelentFeedback");

            migrationBuilder.DropTable(
                name: "UserProfile");

            migrationBuilder.DropTable(
                name: "UserProfileDetail");

            migrationBuilder.DropTable(
                name: "VideoGelleryVideos");

            migrationBuilder.DropTable(
                name: "State");

            migrationBuilder.DropTable(
                name: "TelentRequest");

            migrationBuilder.DropTable(
                name: "ImageGellery");

            migrationBuilder.DropTable(
                name: "VideoGellery");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Category");
        }
    }
}
