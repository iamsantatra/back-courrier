using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace back_courrier.Migrations
{
    /// <inheritdoc />
    public partial class FixForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departement", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Poste",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Poste", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Statut",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statut", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Utilisateur",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MotDePasse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PosteId = table.Column<int>(type: "int", nullable: false),
                    DepartementId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilisateur", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Utilisateur_Departement_DepartementId",
                        column: x => x.DepartementId,
                        principalTable: "Departement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Utilisateur_Poste_PosteId",
                        column: x => x.PosteId,
                        principalTable: "Poste",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Courrier",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Objet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Expediteur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Flag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Commentaire = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceptionnisteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courrier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courrier_Utilisateur_ReceptionnisteId",
                        column: x => x.ReceptionnisteId,
                        principalTable: "Utilisateur",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourrierDestinataire",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartementDestinataireId = table.Column<int>(type: "int", nullable: false),
                    CourrierId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourrierDestinataire", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourrierDestinataire_Courrier_CourrierId",
                        column: x => x.CourrierId,
                        principalTable: "Courrier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourrierDestinataire_Departement_DepartementDestinataireId",
                        column: x => x.DepartementDestinataireId,
                        principalTable: "Departement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Historique",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateHistorique = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CourrierDestinataireId = table.Column<int>(type: "int", nullable: false),
                    StatutId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historique", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Historique_CourrierDestinataire_CourrierDestinataireId",
                        column: x => x.CourrierDestinataireId,
                        principalTable: "CourrierDestinataire",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Historique_Statut_StatutId",
                        column: x => x.StatutId,
                        principalTable: "Statut",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Departement",
                columns: new[] { "Id", "Designation" },
                values: new object[,]
                {
                    { 1, "Ressource humaine" },
                    { 2, "Finance" },
                    { 3, "SI" }
                });

            migrationBuilder.InsertData(
                table: "Poste",
                columns: new[] { "Id", "Designation" },
                values: new object[,]
                {
                    { 1, "receptionniste" },
                    { 2, "coursier" },
                    { 3, "secretaire" },
                    { 4, "directeur" }
                });

            migrationBuilder.InsertData(
                table: "Statut",
                columns: new[] { "Id", "Designation" },
                values: new object[,]
                {
                    { 1, "reçu par le receptionniste" },
                    { 2, "transferé au coursier" },
                    { 3, "transferé au sécrétaire" },
                    { 4, "livré" }
                });

            migrationBuilder.InsertData(
                table: "Utilisateur",
                columns: new[] { "Id", "DepartementId", "MotDePasse", "Nom", "PosteId" },
                values: new object[,]
                {
                    { 1, 1, "1234", "receptionniste", 1 },
                    { 2, 1, "1234", "coursier", 2 },
                    { 3, 1, "1234", "secretaire", 3 },
                    { 4, 1, "1234", "directeur", 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courrier_ReceptionnisteId",
                table: "Courrier",
                column: "ReceptionnisteId");

            migrationBuilder.CreateIndex(
                name: "IX_CourrierDestinataire_CourrierId",
                table: "CourrierDestinataire",
                column: "CourrierId");

            migrationBuilder.CreateIndex(
                name: "IX_CourrierDestinataire_DepartementDestinataireId",
                table: "CourrierDestinataire",
                column: "DepartementDestinataireId");

            migrationBuilder.CreateIndex(
                name: "IX_Historique_CourrierDestinataireId",
                table: "Historique",
                column: "CourrierDestinataireId");

            migrationBuilder.CreateIndex(
                name: "IX_Historique_StatutId",
                table: "Historique",
                column: "StatutId");

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateur_DepartementId",
                table: "Utilisateur",
                column: "DepartementId");

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateur_PosteId",
                table: "Utilisateur",
                column: "PosteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Historique");

            migrationBuilder.DropTable(
                name: "CourrierDestinataire");

            migrationBuilder.DropTable(
                name: "Statut");

            migrationBuilder.DropTable(
                name: "Courrier");

            migrationBuilder.DropTable(
                name: "Utilisateur");

            migrationBuilder.DropTable(
                name: "Departement");

            migrationBuilder.DropTable(
                name: "Poste");
        }
    }
}
