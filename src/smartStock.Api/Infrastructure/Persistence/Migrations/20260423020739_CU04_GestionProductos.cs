using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace smartStock.Api.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CU04_GestionProductos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovimientoStock_ItemDetalleCompra_ItemCompraId",
                table: "MovimientoStock");

            migrationBuilder.DropForeignKey(
                name: "FK_MovimientoStock_ItemDetalleVenta_ItemVentaId",
                table: "MovimientoStock");

            migrationBuilder.DropForeignKey(
                name: "FK_MovimientoStock_Productos_ProductoId",
                table: "MovimientoStock");

            migrationBuilder.DropForeignKey(
                name: "FK_MovimientoStock_Usuarios_UsuarioId",
                table: "MovimientoStock");

            migrationBuilder.DropForeignKey(
                name: "FK_StockActual_Productos_ProductoId",
                table: "StockActual");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StockActual",
                table: "StockActual");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovimientoStock",
                table: "MovimientoStock");

            migrationBuilder.RenameTable(
                name: "StockActual",
                newName: "StocksActuales");

            migrationBuilder.RenameTable(
                name: "MovimientoStock",
                newName: "MovimientosStock");

            migrationBuilder.RenameIndex(
                name: "IX_MovimientoStock_UsuarioId",
                table: "MovimientosStock",
                newName: "IX_MovimientosStock_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_MovimientoStock_ProductoId",
                table: "MovimientosStock",
                newName: "IX_MovimientosStock_ProductoId");

            migrationBuilder.RenameIndex(
                name: "IX_MovimientoStock_ItemVentaId",
                table: "MovimientosStock",
                newName: "IX_MovimientosStock_ItemVentaId");

            migrationBuilder.RenameIndex(
                name: "IX_MovimientoStock_ItemCompraId",
                table: "MovimientosStock",
                newName: "IX_MovimientosStock_ItemCompraId");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Productos",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<bool>(
                name: "EstaActivo",
                table: "Productos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaAlta",
                table: "Productos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "StockMinimo",
                table: "Productos",
                type: "decimal(12,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "UnidadMedida",
                table: "Productos",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StocksActuales",
                table: "StocksActuales",
                column: "ProductoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovimientosStock",
                table: "MovimientosStock",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CodigosProducto",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Factor = table.Column<decimal>(type: "decimal(10,4)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProductoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodigosProducto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodigosProducto_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CodigosProducto_ProductoId",
                table: "CodigosProducto",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "UX_CodigosProducto_Codigo",
                table: "CodigosProducto",
                column: "Codigo",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MovimientosStock_ItemDetalleCompra_ItemCompraId",
                table: "MovimientosStock",
                column: "ItemCompraId",
                principalTable: "ItemDetalleCompra",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovimientosStock_ItemDetalleVenta_ItemVentaId",
                table: "MovimientosStock",
                column: "ItemVentaId",
                principalTable: "ItemDetalleVenta",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovimientosStock_Productos_ProductoId",
                table: "MovimientosStock",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovimientosStock_Usuarios_UsuarioId",
                table: "MovimientosStock",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StocksActuales_Productos_ProductoId",
                table: "StocksActuales",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovimientosStock_ItemDetalleCompra_ItemCompraId",
                table: "MovimientosStock");

            migrationBuilder.DropForeignKey(
                name: "FK_MovimientosStock_ItemDetalleVenta_ItemVentaId",
                table: "MovimientosStock");

            migrationBuilder.DropForeignKey(
                name: "FK_MovimientosStock_Productos_ProductoId",
                table: "MovimientosStock");

            migrationBuilder.DropForeignKey(
                name: "FK_MovimientosStock_Usuarios_UsuarioId",
                table: "MovimientosStock");

            migrationBuilder.DropForeignKey(
                name: "FK_StocksActuales_Productos_ProductoId",
                table: "StocksActuales");

            migrationBuilder.DropTable(
                name: "CodigosProducto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StocksActuales",
                table: "StocksActuales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovimientosStock",
                table: "MovimientosStock");

            migrationBuilder.DropColumn(
                name: "EstaActivo",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "FechaAlta",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "StockMinimo",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "UnidadMedida",
                table: "Productos");

            migrationBuilder.RenameTable(
                name: "StocksActuales",
                newName: "StockActual");

            migrationBuilder.RenameTable(
                name: "MovimientosStock",
                newName: "MovimientoStock");

            migrationBuilder.RenameIndex(
                name: "IX_MovimientosStock_UsuarioId",
                table: "MovimientoStock",
                newName: "IX_MovimientoStock_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_MovimientosStock_ProductoId",
                table: "MovimientoStock",
                newName: "IX_MovimientoStock_ProductoId");

            migrationBuilder.RenameIndex(
                name: "IX_MovimientosStock_ItemVentaId",
                table: "MovimientoStock",
                newName: "IX_MovimientoStock_ItemVentaId");

            migrationBuilder.RenameIndex(
                name: "IX_MovimientosStock_ItemCompraId",
                table: "MovimientoStock",
                newName: "IX_MovimientoStock_ItemCompraId");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Productos",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StockActual",
                table: "StockActual",
                column: "ProductoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovimientoStock",
                table: "MovimientoStock",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovimientoStock_ItemDetalleCompra_ItemCompraId",
                table: "MovimientoStock",
                column: "ItemCompraId",
                principalTable: "ItemDetalleCompra",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovimientoStock_ItemDetalleVenta_ItemVentaId",
                table: "MovimientoStock",
                column: "ItemVentaId",
                principalTable: "ItemDetalleVenta",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovimientoStock_Productos_ProductoId",
                table: "MovimientoStock",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovimientoStock_Usuarios_UsuarioId",
                table: "MovimientoStock",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockActual_Productos_ProductoId",
                table: "StockActual",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
