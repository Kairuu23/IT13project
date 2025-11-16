using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project.Migrations
{
    /// <inheritdoc />
    public partial class AddMemberPromoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MemberPromos",
                columns: table => new
                {
                    MemberPromoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberID = table.Column<int>(type: "int", nullable: false),
                    PromoID = table.Column<int>(type: "int", nullable: false),
                    DateJoined = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PromotionPromoID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberPromos", x => x.MemberPromoID);
                    table.ForeignKey(
                        name: "FK_MemberPromos_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberPromos_Promotions_PromotionPromoID",
                        column: x => x.PromotionPromoID,
                        principalTable: "Promotions",
                        principalColumn: "PromoID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberPromos_MemberID",
                table: "MemberPromos",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberPromos_PromotionPromoID",
                table: "MemberPromos",
                column: "PromotionPromoID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberPromos");
        }
    }
}
