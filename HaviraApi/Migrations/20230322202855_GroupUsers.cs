using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaviraApi.Migrations
{
    /// <inheritdoc />
    public partial class GroupUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Groups_GroupId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_GroupId",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "UserProfiles");

            migrationBuilder.CreateTable(
                name: "GroupUserProfile",
                columns: table => new
                {
                    GroupsId = table.Column<long>(type: "bigint", nullable: false),
                    UserProfilesId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupUserProfile", x => new { x.GroupsId, x.UserProfilesId });
                    table.ForeignKey(
                        name: "FK_GroupUserProfile_Groups_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupUserProfile_UserProfiles_UserProfilesId",
                        column: x => x.UserProfilesId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupUserProfile_UserProfilesId",
                table: "GroupUserProfile",
                column: "UserProfilesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupUserProfile");

            migrationBuilder.AddColumn<long>(
                name: "GroupId",
                table: "UserProfiles",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_GroupId",
                table: "UserProfiles",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Groups_GroupId",
                table: "UserProfiles",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }
    }
}
