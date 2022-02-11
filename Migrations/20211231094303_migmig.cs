using Microsoft.EntityFrameworkCore.Migrations;

namespace MyApi1.Migrations
{
    public partial class migmig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    ParentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Desc = table.Column<string>(nullable: true),
                    VidUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    WhatsApp = table.Column<string>(nullable: true),
                    Instagram = table.Column<string>(nullable: true),
                    PicUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    VidLink = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Setting",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BannerPicUrl1 = table.Column<string>(nullable: true),
                    BannerPicUrl2 = table.Column<string>(nullable: true),
                    CustomerCount = table.Column<string>(nullable: true),
                    ProjectCount = table.Column<string>(nullable: true),
                    TownCount = table.Column<string>(nullable: true),
                    ZoneCount = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sliders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Desc = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    PicUrl = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sliders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Estates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Region = table.Column<string>(nullable: true),
                    LandArea = table.Column<int>(nullable: false),
                    BuildingArea = table.Column<int>(nullable: false),
                    Bedroom = table.Column<int>(nullable: false),
                    Peyment = table.Column<string>(nullable: true),
                    Price = table.Column<int>(nullable: false),
                    Lat = table.Column<double>(nullable: false),
                    Lang = table.Column<double>(nullable: false),
                    Desc = table.Column<string>(nullable: true),
                    Code = table.Column<int>(nullable: false),
                    VidUrl = table.Column<string>(nullable: true),
                    BuildingType = table.Column<int>(nullable: false),
                    CityId = table.Column<int>(nullable: false),
                    ProvinceId = table.Column<int>(nullable: false),
                    ProvinceId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Estates_City_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Estates_City_ProvinceId1",
                        column: x => x.ProvinceId1,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Towns",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Desc = table.Column<string>(nullable: true),
                    Peyment = table.Column<string>(nullable: true),
                    UtmPicUrl = table.Column<string>(nullable: true),
                    Lat = table.Column<double>(nullable: false),
                    Lang = table.Column<double>(nullable: false),
                    VidUrl = table.Column<string>(nullable: true),
                    CityId = table.Column<int>(nullable: false),
                    ProvinceId = table.Column<int>(nullable: false),
                    ProvinceId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Towns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Towns_City_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Towns_City_ProvinceId1",
                        column: x => x.ProvinceId1,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Zones",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Area = table.Column<string>(nullable: true),
                    Desc = table.Column<string>(nullable: true),
                    PicUrl = table.Column<string>(nullable: true),
                    VidUrl = table.Column<string>(nullable: true),
                    CityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zones_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EstateGalleries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PicUrl = table.Column<string>(nullable: true),
                    EstateId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstateGalleries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstateGalleries_Estates_EstateId",
                        column: x => x.EstateId,
                        principalTable: "Estates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TownGalleries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PicUrl = table.Column<string>(nullable: true),
                    TownId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TownGalleries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TownGalleries_Towns_TownId",
                        column: x => x.TownId,
                        principalTable: "Towns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EstateGalleries_EstateId",
                table: "EstateGalleries",
                column: "EstateId");

            migrationBuilder.CreateIndex(
                name: "IX_Estates_ProvinceId",
                table: "Estates",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Estates_ProvinceId1",
                table: "Estates",
                column: "ProvinceId1");

            migrationBuilder.CreateIndex(
                name: "IX_TownGalleries_TownId",
                table: "TownGalleries",
                column: "TownId");

            migrationBuilder.CreateIndex(
                name: "IX_Towns_ProvinceId",
                table: "Towns",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Towns_ProvinceId1",
                table: "Towns",
                column: "ProvinceId1");

            migrationBuilder.CreateIndex(
                name: "IX_Zones_CityId",
                table: "Zones",
                column: "CityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "EstateGalleries");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "Setting");

            migrationBuilder.DropTable(
                name: "Sliders");

            migrationBuilder.DropTable(
                name: "TownGalleries");

            migrationBuilder.DropTable(
                name: "Zones");

            migrationBuilder.DropTable(
                name: "Estates");

            migrationBuilder.DropTable(
                name: "Towns");

            migrationBuilder.DropTable(
                name: "City");
        }
    }
}
