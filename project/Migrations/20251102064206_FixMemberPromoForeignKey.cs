using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project.Migrations
{
    /// <inheritdoc />
    public partial class FixMemberPromoForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberPromos_Promotions_PromotionPromoID",
                table: "MemberPromos");

            migrationBuilder.DropIndex(
                name: "IX_MemberPromos_PromotionPromoID",
                table: "MemberPromos");

            migrationBuilder.DropColumn(
                name: "PromotionPromoID",
                table: "MemberPromos");

            migrationBuilder.CreateIndex(
                name: "IX_MemberPromos_PromoID",
                table: "MemberPromos",
                column: "PromoID");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberPromos_Promotions_PromoID",
                table: "MemberPromos",
                column: "PromoID",
                principalTable: "Promotions",
                principalColumn: "PromoID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberPromos_Promotions_PromoID",
                table: "MemberPromos");

            migrationBuilder.DropIndex(
                name: "IX_MemberPromos_PromoID",
                table: "MemberPromos");

            migrationBuilder.AddColumn<int>(
                name: "PromotionPromoID",
                table: "MemberPromos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemberPromos_PromotionPromoID",
                table: "MemberPromos",
                column: "PromotionPromoID");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberPromos_Promotions_PromotionPromoID",
                table: "MemberPromos",
                column: "PromotionPromoID",
                principalTable: "Promotions",
                principalColumn: "PromoID");
        }
    }
}
