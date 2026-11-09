using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourtChain.Api.Migrations
{
    /// <inheritdoc />
    public partial class AdministrationRevisions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Facilities",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Facilities",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Courts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Rationale",
                table: "Audits",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Policies_Scope_FacilityId_Sport_EffectiveStart",
                table: "Policies",
                columns: new[] { "Scope", "FacilityId", "Sport", "EffectiveStart" });

            migrationBuilder.CreateIndex(
                name: "IX_Audits_Entity_EntityId_At",
                table: "Audits",
                columns: new[] { "Entity", "EntityId", "At" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Policies_Scope_FacilityId_Sport_EffectiveStart",
                table: "Policies");

            migrationBuilder.DropIndex(
                name: "IX_Audits_Entity_EntityId_At",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Facilities");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Facilities");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Courts");

            migrationBuilder.DropColumn(
                name: "Rationale",
                table: "Audits");
        }
    }
}
