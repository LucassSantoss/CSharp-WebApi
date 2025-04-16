using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApi.Repo.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Diretores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diretores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Filmes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titulo = table.Column<string>(type: "TEXT", nullable: false),
                    Ano = table.Column<int>(type: "INTEGER", nullable: false),
                    DiretorId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filmes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Filmes_Diretores_DiretorId",
                        column: x => x.DiretorId,
                        principalTable: "Diretores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Diretores",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Christopher Nolan" },
                    { 2, "Steven Spielberg" },
                    { 3, "Martin Scorsese" },
                    { 4, "Quentin Tarantino" },
                    { 5, "Greta Gerwig" }
                });

            migrationBuilder.InsertData(
                table: "Filmes",
                columns: new[] { "Id", "Ano", "DiretorId", "Titulo" },
                values: new object[,]
                {
                    { 1, 2010, 1, "Inception" },
                    { 2, 2014, 1, "Interstellar" },
                    { 3, 1993, 2, "Jurassic Park" },
                    { 4, 1982, 2, "E.T. the Extra-Terrestrial" },
                    { 5, 2013, 3, "The Wolf of Wall Street" },
                    { 6, 2010, 3, "Shutter Island" },
                    { 7, 1994, 4, "Pulp Fiction" },
                    { 8, 2019, 4, "Once Upon a Time in Hollywood" },
                    { 9, 2017, 5, "Lady Bird" },
                    { 10, 2023, 5, "Barbie" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Filmes_DiretorId",
                table: "Filmes",
                column: "DiretorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Filmes");

            migrationBuilder.DropTable(
                name: "Diretores");
        }
    }
}
