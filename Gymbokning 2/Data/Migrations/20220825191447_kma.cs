using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gymbokning_2.Data.Migrations
{
    public partial class kma : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserGyms_GymClass_GymClassId",
                table: "ApplicationUserGyms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GymClass",
                table: "GymClass");

            migrationBuilder.RenameTable(
                name: "GymClass",
                newName: "GymClasses");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "GymClasses",
                newName: "StartDate");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GymClasses",
                table: "GymClasses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserGyms_GymClasses_GymClassId",
                table: "ApplicationUserGyms",
                column: "GymClassId",
                principalTable: "GymClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserGyms_GymClasses_GymClassId",
                table: "ApplicationUserGyms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GymClasses",
                table: "GymClasses");

            migrationBuilder.RenameTable(
                name: "GymClasses",
                newName: "GymClass");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "GymClass",
                newName: "StartTime");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GymClass",
                table: "GymClass",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserGyms_GymClass_GymClassId",
                table: "ApplicationUserGyms",
                column: "GymClassId",
                principalTable: "GymClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
