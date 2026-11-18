using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViaCidada.Ouvidoria.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_VC_NET_REGIOES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    ZonaCidade = table.Column<string>(type: "NVARCHAR2(40)", maxLength: 40, nullable: false),
                    Subprefeitura = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_VC_NET_REGIOES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_VC_NET_PROPOSTAS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Titulo = table.Column<string>(type: "NVARCHAR2(120)", maxLength: 120, nullable: false),
                    Descricao = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    Categoria = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    CodigoLinhaOuParada = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: true),
                    Status = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    RegiaoUrbanaId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_VC_NET_PROPOSTAS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_VC_NET_PROPOSTAS_TB_VC_NET_REGIOES_RegiaoUrbanaId",
                        column: x => x.RegiaoUrbanaId,
                        principalTable: "TB_VC_NET_REGIOES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_VC_NET_PROPOSTAS_RegiaoUrbanaId",
                table: "TB_VC_NET_PROPOSTAS",
                column: "RegiaoUrbanaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_VC_NET_PROPOSTAS");

            migrationBuilder.DropTable(
                name: "TB_VC_NET_REGIOES");
        }
    }
}
