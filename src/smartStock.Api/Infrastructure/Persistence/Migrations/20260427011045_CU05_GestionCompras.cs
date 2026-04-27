using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace smartStock.Api.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CU05_GestionCompras : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CierreCaja_CompraDia_CompraDiaId",
                table: "CierreCaja");

            migrationBuilder.DropForeignKey(
                name: "FK_CierreCaja_VentaDia_VentaDiaId",
                table: "CierreCaja");

            migrationBuilder.DropForeignKey(
                name: "FK_CompraDia_Proveedores_ProveedorId",
                table: "CompraDia");

            migrationBuilder.DropForeignKey(
                name: "FK_CompraDia_Usuarios_UsuarioId",
                table: "CompraDia");

            migrationBuilder.DropForeignKey(
                name: "FK_DetalleCompra_CompraDia_CompraDiaId",
                table: "DetalleCompra");

            migrationBuilder.DropForeignKey(
                name: "FK_DetalleCompra_Usuarios_UsuarioId",
                table: "DetalleCompra");

            migrationBuilder.DropForeignKey(
                name: "FK_DetalleVenta_Usuarios_UsuarioId",
                table: "DetalleVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_DetalleVenta_VentaDia_VentaDiaId",
                table: "DetalleVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemDetalleCompra_DetalleCompra_DetalleCompraId",
                table: "ItemDetalleCompra");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemDetalleCompra_Productos_ProductoId",
                table: "ItemDetalleCompra");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemDetalleVenta_DetalleVenta_DetalleVentaId",
                table: "ItemDetalleVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemDetalleVenta_Productos_ProductoId",
                table: "ItemDetalleVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_MovimientosStock_ItemDetalleCompra_ItemCompraId",
                table: "MovimientosStock");

            migrationBuilder.DropForeignKey(
                name: "FK_MovimientosStock_ItemDetalleVenta_ItemVentaId",
                table: "MovimientosStock");

            migrationBuilder.DropForeignKey(
                name: "FK_VentaDia_Usuarios_UsuarioId",
                table: "VentaDia");

            migrationBuilder.DropIndex(
                name: "IX_MovimientosStock_ItemCompraId",
                table: "MovimientosStock");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VentaDia",
                table: "VentaDia");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemDetalleVenta",
                table: "ItemDetalleVenta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemDetalleCompra",
                table: "ItemDetalleCompra");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DetalleVenta",
                table: "DetalleVenta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DetalleCompra",
                table: "DetalleCompra");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompraDia",
                table: "CompraDia");

            migrationBuilder.DropIndex(
                name: "IX_CompraDia_ProveedorId",
                table: "CompraDia");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CierreCaja",
                table: "CierreCaja");

            migrationBuilder.DropColumn(
                name: "ProveedorId",
                table: "CompraDia");

            migrationBuilder.RenameTable(
                name: "VentaDia",
                newName: "VentasDia");

            migrationBuilder.RenameTable(
                name: "ItemDetalleVenta",
                newName: "ItemsDetalleVenta");

            migrationBuilder.RenameTable(
                name: "ItemDetalleCompra",
                newName: "ItemsDetalleCompra");

            migrationBuilder.RenameTable(
                name: "DetalleVenta",
                newName: "DetallesVenta");

            migrationBuilder.RenameTable(
                name: "DetalleCompra",
                newName: "DetallesCompra");

            migrationBuilder.RenameTable(
                name: "CompraDia",
                newName: "ComprasDia");

            migrationBuilder.RenameTable(
                name: "CierreCaja",
                newName: "CierresCaja");

            migrationBuilder.RenameIndex(
                name: "IX_VentaDia_UsuarioId",
                table: "VentasDia",
                newName: "IX_VentasDia_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemDetalleVenta_ProductoId",
                table: "ItemsDetalleVenta",
                newName: "IX_ItemsDetalleVenta_ProductoId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemDetalleVenta_DetalleVentaId",
                table: "ItemsDetalleVenta",
                newName: "IX_ItemsDetalleVenta_DetalleVentaId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemDetalleCompra_ProductoId",
                table: "ItemsDetalleCompra",
                newName: "IX_ItemsDetalleCompra_ProductoId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemDetalleCompra_DetalleCompraId",
                table: "ItemsDetalleCompra",
                newName: "IX_ItemsDetalleCompra_DetalleCompraId");

            migrationBuilder.RenameIndex(
                name: "IX_DetalleVenta_VentaDiaId",
                table: "DetallesVenta",
                newName: "IX_DetallesVenta_VentaDiaId");

            migrationBuilder.RenameIndex(
                name: "IX_DetalleVenta_UsuarioId",
                table: "DetallesVenta",
                newName: "IX_DetallesVenta_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_DetalleCompra_UsuarioId",
                table: "DetallesCompra",
                newName: "IX_DetallesCompra_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_DetalleCompra_CompraDiaId",
                table: "DetallesCompra",
                newName: "IX_DetallesCompra_CompraDiaId");

            migrationBuilder.RenameIndex(
                name: "IX_CompraDia_UsuarioId",
                table: "ComprasDia",
                newName: "IX_ComprasDia_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_CierreCaja_VentaDiaId",
                table: "CierresCaja",
                newName: "IX_CierresCaja_VentaDiaId");

            migrationBuilder.RenameIndex(
                name: "IX_CierreCaja_CompraDiaId",
                table: "CierresCaja",
                newName: "IX_CierresCaja_CompraDiaId");

            migrationBuilder.AlterColumn<string>(
                name: "Tipo",
                table: "MovimientosStock",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Observacion",
                table: "MovimientosStock",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CodigoProductoId",
                table: "ItemsDetalleCompra",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Factor",
                table: "ItemsDetalleCompra",
                type: "decimal(12,4)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreProducto",
                table: "ItemsDetalleCompra",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "EstaAnulada",
                table: "DetallesCompra",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaAnulacion",
                table: "DetallesCompra",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCompra",
                table: "DetallesCompra",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaComprobante",
                table: "DetallesCompra",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoAnulacion",
                table: "DetallesCompra",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroComprobante",
                table: "DetallesCompra",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProveedorId",
                table: "DetallesCompra",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "TipoComprobante",
                table: "DetallesCompra",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "DetallesCompra",
                type: "decimal(12,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioAnulaId",
                table: "DetallesCompra",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "ComprasDia",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VentasDia",
                table: "VentasDia",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemsDetalleVenta",
                table: "ItemsDetalleVenta",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemsDetalleCompra",
                table: "ItemsDetalleCompra",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetallesVenta",
                table: "DetallesVenta",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetallesCompra",
                table: "DetallesCompra",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComprasDia",
                table: "ComprasDia",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CierresCaja",
                table: "CierresCaja",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosStock_ItemCompraId",
                table: "MovimientosStock",
                column: "ItemCompraId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsDetalleCompra_CodigoProductoId",
                table: "ItemsDetalleCompra",
                column: "CodigoProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesCompra_UsuarioAnulaId",
                table: "DetallesCompra",
                column: "UsuarioAnulaId");

            migrationBuilder.CreateIndex(
                name: "UX_DetallesCompra_Comprobante",
                table: "DetallesCompra",
                columns: new[] { "ProveedorId", "NumeroComprobante", "TipoComprobante" },
                unique: true,
                filter: "[NumeroComprobante] IS NOT NULL AND [EstaAnulada] = 0");

            migrationBuilder.AddForeignKey(
                name: "FK_CierresCaja_ComprasDia_CompraDiaId",
                table: "CierresCaja",
                column: "CompraDiaId",
                principalTable: "ComprasDia",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CierresCaja_VentasDia_VentaDiaId",
                table: "CierresCaja",
                column: "VentaDiaId",
                principalTable: "VentasDia",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ComprasDia_Usuarios_UsuarioId",
                table: "ComprasDia",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesCompra_ComprasDia_CompraDiaId",
                table: "DetallesCompra",
                column: "CompraDiaId",
                principalTable: "ComprasDia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesCompra_Proveedores_ProveedorId",
                table: "DetallesCompra",
                column: "ProveedorId",
                principalTable: "Proveedores",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesCompra_Usuarios_UsuarioAnulaId",
                table: "DetallesCompra",
                column: "UsuarioAnulaId",
                principalTable: "Usuarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesCompra_Usuarios_UsuarioId",
                table: "DetallesCompra",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesVenta_Usuarios_UsuarioId",
                table: "DetallesVenta",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesVenta_VentasDia_VentaDiaId",
                table: "DetallesVenta",
                column: "VentaDiaId",
                principalTable: "VentasDia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemsDetalleCompra_CodigosProducto_CodigoProductoId",
                table: "ItemsDetalleCompra",
                column: "CodigoProductoId",
                principalTable: "CodigosProducto",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemsDetalleCompra_DetallesCompra_DetalleCompraId",
                table: "ItemsDetalleCompra",
                column: "DetalleCompraId",
                principalTable: "DetallesCompra",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemsDetalleCompra_Productos_ProductoId",
                table: "ItemsDetalleCompra",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemsDetalleVenta_DetallesVenta_DetalleVentaId",
                table: "ItemsDetalleVenta",
                column: "DetalleVentaId",
                principalTable: "DetallesVenta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemsDetalleVenta_Productos_ProductoId",
                table: "ItemsDetalleVenta",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovimientosStock_ItemsDetalleCompra_ItemCompraId",
                table: "MovimientosStock",
                column: "ItemCompraId",
                principalTable: "ItemsDetalleCompra",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovimientosStock_ItemsDetalleVenta_ItemVentaId",
                table: "MovimientosStock",
                column: "ItemVentaId",
                principalTable: "ItemsDetalleVenta",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VentasDia_Usuarios_UsuarioId",
                table: "VentasDia",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CierresCaja_ComprasDia_CompraDiaId",
                table: "CierresCaja");

            migrationBuilder.DropForeignKey(
                name: "FK_CierresCaja_VentasDia_VentaDiaId",
                table: "CierresCaja");

            migrationBuilder.DropForeignKey(
                name: "FK_ComprasDia_Usuarios_UsuarioId",
                table: "ComprasDia");

            migrationBuilder.DropForeignKey(
                name: "FK_DetallesCompra_ComprasDia_CompraDiaId",
                table: "DetallesCompra");

            migrationBuilder.DropForeignKey(
                name: "FK_DetallesCompra_Proveedores_ProveedorId",
                table: "DetallesCompra");

            migrationBuilder.DropForeignKey(
                name: "FK_DetallesCompra_Usuarios_UsuarioAnulaId",
                table: "DetallesCompra");

            migrationBuilder.DropForeignKey(
                name: "FK_DetallesCompra_Usuarios_UsuarioId",
                table: "DetallesCompra");

            migrationBuilder.DropForeignKey(
                name: "FK_DetallesVenta_Usuarios_UsuarioId",
                table: "DetallesVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_DetallesVenta_VentasDia_VentaDiaId",
                table: "DetallesVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemsDetalleCompra_CodigosProducto_CodigoProductoId",
                table: "ItemsDetalleCompra");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemsDetalleCompra_DetallesCompra_DetalleCompraId",
                table: "ItemsDetalleCompra");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemsDetalleCompra_Productos_ProductoId",
                table: "ItemsDetalleCompra");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemsDetalleVenta_DetallesVenta_DetalleVentaId",
                table: "ItemsDetalleVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemsDetalleVenta_Productos_ProductoId",
                table: "ItemsDetalleVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_MovimientosStock_ItemsDetalleCompra_ItemCompraId",
                table: "MovimientosStock");

            migrationBuilder.DropForeignKey(
                name: "FK_MovimientosStock_ItemsDetalleVenta_ItemVentaId",
                table: "MovimientosStock");

            migrationBuilder.DropForeignKey(
                name: "FK_VentasDia_Usuarios_UsuarioId",
                table: "VentasDia");

            migrationBuilder.DropIndex(
                name: "IX_MovimientosStock_ItemCompraId",
                table: "MovimientosStock");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VentasDia",
                table: "VentasDia");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemsDetalleVenta",
                table: "ItemsDetalleVenta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemsDetalleCompra",
                table: "ItemsDetalleCompra");

            migrationBuilder.DropIndex(
                name: "IX_ItemsDetalleCompra_CodigoProductoId",
                table: "ItemsDetalleCompra");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DetallesVenta",
                table: "DetallesVenta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DetallesCompra",
                table: "DetallesCompra");

            migrationBuilder.DropIndex(
                name: "IX_DetallesCompra_UsuarioAnulaId",
                table: "DetallesCompra");

            migrationBuilder.DropIndex(
                name: "UX_DetallesCompra_Comprobante",
                table: "DetallesCompra");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ComprasDia",
                table: "ComprasDia");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CierresCaja",
                table: "CierresCaja");

            migrationBuilder.DropColumn(
                name: "Observacion",
                table: "MovimientosStock");

            migrationBuilder.DropColumn(
                name: "CodigoProductoId",
                table: "ItemsDetalleCompra");

            migrationBuilder.DropColumn(
                name: "Factor",
                table: "ItemsDetalleCompra");

            migrationBuilder.DropColumn(
                name: "NombreProducto",
                table: "ItemsDetalleCompra");

            migrationBuilder.DropColumn(
                name: "EstaAnulada",
                table: "DetallesCompra");

            migrationBuilder.DropColumn(
                name: "FechaAnulacion",
                table: "DetallesCompra");

            migrationBuilder.DropColumn(
                name: "FechaCompra",
                table: "DetallesCompra");

            migrationBuilder.DropColumn(
                name: "FechaComprobante",
                table: "DetallesCompra");

            migrationBuilder.DropColumn(
                name: "MotivoAnulacion",
                table: "DetallesCompra");

            migrationBuilder.DropColumn(
                name: "NumeroComprobante",
                table: "DetallesCompra");

            migrationBuilder.DropColumn(
                name: "ProveedorId",
                table: "DetallesCompra");

            migrationBuilder.DropColumn(
                name: "TipoComprobante",
                table: "DetallesCompra");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "DetallesCompra");

            migrationBuilder.DropColumn(
                name: "UsuarioAnulaId",
                table: "DetallesCompra");

            migrationBuilder.RenameTable(
                name: "VentasDia",
                newName: "VentaDia");

            migrationBuilder.RenameTable(
                name: "ItemsDetalleVenta",
                newName: "ItemDetalleVenta");

            migrationBuilder.RenameTable(
                name: "ItemsDetalleCompra",
                newName: "ItemDetalleCompra");

            migrationBuilder.RenameTable(
                name: "DetallesVenta",
                newName: "DetalleVenta");

            migrationBuilder.RenameTable(
                name: "DetallesCompra",
                newName: "DetalleCompra");

            migrationBuilder.RenameTable(
                name: "ComprasDia",
                newName: "CompraDia");

            migrationBuilder.RenameTable(
                name: "CierresCaja",
                newName: "CierreCaja");

            migrationBuilder.RenameIndex(
                name: "IX_VentasDia_UsuarioId",
                table: "VentaDia",
                newName: "IX_VentaDia_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemsDetalleVenta_ProductoId",
                table: "ItemDetalleVenta",
                newName: "IX_ItemDetalleVenta_ProductoId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemsDetalleVenta_DetalleVentaId",
                table: "ItemDetalleVenta",
                newName: "IX_ItemDetalleVenta_DetalleVentaId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemsDetalleCompra_ProductoId",
                table: "ItemDetalleCompra",
                newName: "IX_ItemDetalleCompra_ProductoId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemsDetalleCompra_DetalleCompraId",
                table: "ItemDetalleCompra",
                newName: "IX_ItemDetalleCompra_DetalleCompraId");

            migrationBuilder.RenameIndex(
                name: "IX_DetallesVenta_VentaDiaId",
                table: "DetalleVenta",
                newName: "IX_DetalleVenta_VentaDiaId");

            migrationBuilder.RenameIndex(
                name: "IX_DetallesVenta_UsuarioId",
                table: "DetalleVenta",
                newName: "IX_DetalleVenta_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_DetallesCompra_UsuarioId",
                table: "DetalleCompra",
                newName: "IX_DetalleCompra_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_DetallesCompra_CompraDiaId",
                table: "DetalleCompra",
                newName: "IX_DetalleCompra_CompraDiaId");

            migrationBuilder.RenameIndex(
                name: "IX_ComprasDia_UsuarioId",
                table: "CompraDia",
                newName: "IX_CompraDia_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_CierresCaja_VentaDiaId",
                table: "CierreCaja",
                newName: "IX_CierreCaja_VentaDiaId");

            migrationBuilder.RenameIndex(
                name: "IX_CierresCaja_CompraDiaId",
                table: "CierreCaja",
                newName: "IX_CierreCaja_CompraDiaId");

            migrationBuilder.AlterColumn<int>(
                name: "Tipo",
                table: "MovimientosStock",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "Estado",
                table: "CompraDia",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<Guid>(
                name: "ProveedorId",
                table: "CompraDia",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_VentaDia",
                table: "VentaDia",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemDetalleVenta",
                table: "ItemDetalleVenta",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemDetalleCompra",
                table: "ItemDetalleCompra",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetalleVenta",
                table: "DetalleVenta",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetalleCompra",
                table: "DetalleCompra",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompraDia",
                table: "CompraDia",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CierreCaja",
                table: "CierreCaja",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosStock_ItemCompraId",
                table: "MovimientosStock",
                column: "ItemCompraId",
                unique: true,
                filter: "[ItemCompraId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CompraDia_ProveedorId",
                table: "CompraDia",
                column: "ProveedorId");

            migrationBuilder.AddForeignKey(
                name: "FK_CierreCaja_CompraDia_CompraDiaId",
                table: "CierreCaja",
                column: "CompraDiaId",
                principalTable: "CompraDia",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CierreCaja_VentaDia_VentaDiaId",
                table: "CierreCaja",
                column: "VentaDiaId",
                principalTable: "VentaDia",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompraDia_Proveedores_ProveedorId",
                table: "CompraDia",
                column: "ProveedorId",
                principalTable: "Proveedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompraDia_Usuarios_UsuarioId",
                table: "CompraDia",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleCompra_CompraDia_CompraDiaId",
                table: "DetalleCompra",
                column: "CompraDiaId",
                principalTable: "CompraDia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleCompra_Usuarios_UsuarioId",
                table: "DetalleCompra",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleVenta_Usuarios_UsuarioId",
                table: "DetalleVenta",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleVenta_VentaDia_VentaDiaId",
                table: "DetalleVenta",
                column: "VentaDiaId",
                principalTable: "VentaDia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDetalleCompra_DetalleCompra_DetalleCompraId",
                table: "ItemDetalleCompra",
                column: "DetalleCompraId",
                principalTable: "DetalleCompra",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDetalleCompra_Productos_ProductoId",
                table: "ItemDetalleCompra",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDetalleVenta_DetalleVenta_DetalleVentaId",
                table: "ItemDetalleVenta",
                column: "DetalleVentaId",
                principalTable: "DetalleVenta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDetalleVenta_Productos_ProductoId",
                table: "ItemDetalleVenta",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id");

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
                name: "FK_VentaDia_Usuarios_UsuarioId",
                table: "VentaDia",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
