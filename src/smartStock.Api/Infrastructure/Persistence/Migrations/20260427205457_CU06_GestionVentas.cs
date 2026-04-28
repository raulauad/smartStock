using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace smartStock.Api.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CU06_GestionVentas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VentasDia_Usuarios_UsuarioId",
                table: "VentasDia");

            migrationBuilder.DropIndex(
                name: "IX_MovimientosStock_ItemVentaId",
                table: "MovimientosStock");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "VentasDia",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "CodigoProductoId",
                table: "ItemsDetalleVenta",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Factor",
                table: "ItemsDetalleVenta",
                type: "decimal(12,4)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreProducto",
                table: "ItemsDetalleVenta",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "EstaAnulada",
                table: "DetallesVenta",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaAnulacion",
                table: "DetallesVenta",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FormaPago",
                table: "DetallesVenta",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "MontoRecibido",
                table: "DetallesVenta",
                type: "decimal(12,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoAnulacion",
                table: "DetallesVenta",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumeroComprobante",
                table: "DetallesVenta",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "DetallesVenta",
                type: "decimal(12,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioAnulaId",
                table: "DetallesVenta",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "UX_VentasDia_FechaSesion",
                table: "VentasDia",
                column: "FechaSesion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosStock_ItemVentaId",
                table: "MovimientosStock",
                column: "ItemVentaId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsDetalleVenta_CodigoProductoId",
                table: "ItemsDetalleVenta",
                column: "CodigoProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesVenta_UsuarioAnulaId",
                table: "DetallesVenta",
                column: "UsuarioAnulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesVenta_Usuarios_UsuarioAnulaId",
                table: "DetallesVenta",
                column: "UsuarioAnulaId",
                principalTable: "Usuarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemsDetalleVenta_CodigosProducto_CodigoProductoId",
                table: "ItemsDetalleVenta",
                column: "CodigoProductoId",
                principalTable: "CodigosProducto",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VentasDia_Usuarios_UsuarioId",
                table: "VentasDia",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetallesVenta_Usuarios_UsuarioAnulaId",
                table: "DetallesVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemsDetalleVenta_CodigosProducto_CodigoProductoId",
                table: "ItemsDetalleVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_VentasDia_Usuarios_UsuarioId",
                table: "VentasDia");

            migrationBuilder.DropIndex(
                name: "UX_VentasDia_FechaSesion",
                table: "VentasDia");

            migrationBuilder.DropIndex(
                name: "IX_MovimientosStock_ItemVentaId",
                table: "MovimientosStock");

            migrationBuilder.DropIndex(
                name: "IX_ItemsDetalleVenta_CodigoProductoId",
                table: "ItemsDetalleVenta");

            migrationBuilder.DropIndex(
                name: "IX_DetallesVenta_UsuarioAnulaId",
                table: "DetallesVenta");

            migrationBuilder.DropColumn(
                name: "CodigoProductoId",
                table: "ItemsDetalleVenta");

            migrationBuilder.DropColumn(
                name: "Factor",
                table: "ItemsDetalleVenta");

            migrationBuilder.DropColumn(
                name: "NombreProducto",
                table: "ItemsDetalleVenta");

            migrationBuilder.DropColumn(
                name: "EstaAnulada",
                table: "DetallesVenta");

            migrationBuilder.DropColumn(
                name: "FechaAnulacion",
                table: "DetallesVenta");

            migrationBuilder.DropColumn(
                name: "FormaPago",
                table: "DetallesVenta");

            migrationBuilder.DropColumn(
                name: "MontoRecibido",
                table: "DetallesVenta");

            migrationBuilder.DropColumn(
                name: "MotivoAnulacion",
                table: "DetallesVenta");

            migrationBuilder.DropColumn(
                name: "NumeroComprobante",
                table: "DetallesVenta");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "DetallesVenta");

            migrationBuilder.DropColumn(
                name: "UsuarioAnulaId",
                table: "DetallesVenta");

            migrationBuilder.AlterColumn<int>(
                name: "Estado",
                table: "VentasDia",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosStock_ItemVentaId",
                table: "MovimientosStock",
                column: "ItemVentaId",
                unique: true,
                filter: "[ItemVentaId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_VentasDia_Usuarios_UsuarioId",
                table: "VentasDia",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
