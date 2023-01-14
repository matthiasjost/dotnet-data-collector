using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataCollector.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Creator",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryOrSection = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Creator", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChannelEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorEntityId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelEntity_Creator_CreatorEntityId",
                        column: x => x.CreatorEntityId,
                        principalTable: "Creator",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorEntityId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tag_Creator_CreatorEntityId",
                        column: x => x.CreatorEntityId,
                        principalTable: "Creator",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FeedEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChannelEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedEntity_ChannelEntity_ChannelEntityId",
                        column: x => x.ChannelEntityId,
                        principalTable: "ChannelEntity",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelEntity_CreatorEntityId",
                table: "ChannelEntity",
                column: "CreatorEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedEntity_ChannelEntityId",
                table: "FeedEntity",
                column: "ChannelEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_CreatorEntityId",
                table: "Tag",
                column: "CreatorEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeedEntity");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "ChannelEntity");

            migrationBuilder.DropTable(
                name: "Creator");
        }
    }
}
