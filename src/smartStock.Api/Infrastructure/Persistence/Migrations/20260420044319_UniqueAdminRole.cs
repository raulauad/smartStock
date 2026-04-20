using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace smartStock.Api.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UniqueAdminRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cuit",
                table: "Proveedor");

            migrationBuilder.CreateIndex(
                name: "UX_UsuarioRoles_Administrador",
                table: "UsuarioRoles",
                column: "Rol",
                unique: true,
                filter: "[Rol] = 'Administrador'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_UsuarioRoles_Administrador",
                table: "UsuarioRoles");

            migrationBuilder.AddColumn<string>(
                name: "Cuit",
                table: "Proveedor",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
