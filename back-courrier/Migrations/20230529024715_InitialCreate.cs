using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace back_courrier.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
/*            migrationBuilder.CreateTable(
                name: "Poste",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"), // Add this line to enable auto-increment,
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Poste", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"), // Add this line to enable auto-increment,
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departement", x => x.Id);
                });



            migrationBuilder.CreateTable(
                name: "Utilisateur",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"), // Add this line to enable auto-increment,
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MotDePasse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id_Poste = table.Column<int>(type: "int", nullable: false),
                    Id_Departement = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilisateur", x => x.Id);
                    //Add foreign key for Poste
                    table.ForeignKey(
                        name: "FK_Utilisateur_Poste_Id_Poste",
                        column: x => x.Id_Poste,
                        principalTable: "Poste",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    //Add foreign key for Departement
                    table.ForeignKey(
                        name: "FK_Utilisateur_Departement_Id_Departement",
                        column: x => x.Id_Departement,
                        principalTable: "Departement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });*/


           /* migrationBuilder.InsertData(
                table: "Poste",
                columns: new[] { "Designation" },
                values: new object[,]
                {
                                { "Ressource humaine" },
                                { "Finance" },
                                { "SI" }
            });

            migrationBuilder.InsertData(
                table: "Departement",
                columns: new[] { "Designation" },
                values: new object[,]
                {
                                { "receptionniste" },
                                { "coursier" },
                                { "secretaire" },
                                { "directeur" }
                });

            migrationBuilder.InsertData(
                table: "Utilisateur",
                columns: new[] { "Nom", "MotDePasse", "Id_Poste", "Id_Departement" },
                values: new object[,]
                {
                                { "receptionniste", "1234", 1, 1 },
                                { "coursier", "1234", 2, 1 },
                                { "secretaire", "1234", 3, 1 },
                                { "directeur", "1234", 4, 1 }
                });*/
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
/*            migrationBuilder.DropTable(
                name: "Utilisateur");
            migrationBuilder.DropTable(
                name: "Poste");
            migrationBuilder.DropTable(
                name: "Departement");*/
        }
    }
}
