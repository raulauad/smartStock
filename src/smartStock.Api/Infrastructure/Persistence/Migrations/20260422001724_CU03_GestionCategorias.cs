using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace smartStock.Api.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CU03_GestionCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemDetalleCompra_Producto_ProductoId",
                table: "ItemDetalleCompra");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemDetalleVenta_Producto_ProductoId",
                table: "ItemDetalleVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_MovimientoStock_Producto_ProductoId",
                table: "MovimientoStock");

            migrationBuilder.DropForeignKey(
                name: "FK_Producto_Categorias_CategoriaId",
                table: "Producto");

            migrationBuilder.DropForeignKey(
                name: "FK_Producto_Usuarios_UsuarioAltaId",
                table: "Producto");

            migrationBuilder.DropForeignKey(
                name: "FK_StockActual_Producto_ProductoId",
                table: "StockActual");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Producto",
                table: "Producto");

            migrationBuilder.RenameTable(
                name: "Producto",
                newName: "Productos");

            migrationBuilder.RenameIndex(
                name: "IX_Producto_UsuarioAltaId",
                table: "Productos",
                newName: "IX_Productos_UsuarioAltaId");

            migrationBuilder.RenameIndex(
                name: "IX_Producto_CategoriaId",
                table: "Productos",
                newName: "IX_Productos_CategoriaId");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Categorias",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AddColumn<bool>(
                name: "EstaActivo",
                table: "Categorias",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaAlta",
                table: "Categorias",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioAltaId",
                table: "Categorias",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Productos",
                table: "Productos",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_Email",
                table: "Proveedores",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_Nombre",
                table: "Proveedores",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_Telefono",
                table: "Proveedores",
                column: "Telefono",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_UsuarioAltaId",
                table: "Categorias",
                column: "UsuarioAltaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categorias_Usuarios_UsuarioAltaId",
                table: "Categorias",
                column: "UsuarioAltaId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDetalleCompra_Productos_ProductoId",
                table: "ItemDetalleCompra",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDetalleVenta_Productos_ProductoId",
                table: "ItemDetalleVenta",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovimientoStock_Productos_ProductoId",
                table: "MovimientoStock",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Categorias_CategoriaId",
                table: "Productos",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Usuarios_UsuarioAltaId",
                table: "Productos",
                column: "UsuarioAltaId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockActual_Productos_ProductoId",
                table: "StockActual",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categorias_Usuarios_UsuarioAltaId",
                table: "Categorias");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemDetalleCompra_Productos_ProductoId",
                table: "ItemDetalleCompra");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemDetalleVenta_Productos_ProductoId",
                table: "ItemDetalleVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_MovimientoStock_Productos_ProductoId",
                table: "MovimientoStock");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Categorias_CategoriaId",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Usuarios_UsuarioAltaId",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_StockActual_Productos_ProductoId",
                table: "StockActual");

            migrationBuilder.DropIndex(
                name: "IX_Proveedores_Email",
                table: "Proveedores");

            migrationBuilder.DropIndex(
                name: "IX_Proveedores_Nombre",
                table: "Proveedores");

            migrationBuilder.DropIndex(
                name: "IX_Proveedores_Telefono",
                table: "Proveedores");

            migrationBuilder.DropIndex(
                name: "IX_Categorias_UsuarioAltaId",
                table: "Categorias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Productos",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "EstaActivo",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "FechaAlta",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "UsuarioAltaId",
                table: "Categorias");

            migrationBuilder.RenameTable(
                name: "Productos",
                newName: "Producto");

            migrationBuilder.RenameIndex(
                name: "IX_Productos_UsuarioAltaId",
                table: "Producto",
                newName: "IX_Producto_UsuarioAltaId");

            migrationBuilder.RenameIndex(
                name: "IX_Productos_CategoriaId",
                table: "Producto",
                newName: "IX_Producto_CategoriaId");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Categorias",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Producto",
                table: "Producto",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDetalleCompra_Producto_ProductoId",
                table: "ItemDetalleCompra",
                column: "ProductoId",
                principalTable: "Producto",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDetalleVenta_Producto_ProductoId",
                table: "ItemDetalleVenta",
                column: "ProductoId",
                principalTable: "Producto",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovimientoStock_Producto_ProductoId",
                table: "MovimientoStock",
                column: "ProductoId",
                principalTable: "Producto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Producto_Categorias_CategoriaId",
                table: "Producto",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Producto_Usuarios_UsuarioAltaId",
                table: "Producto",
                column: "UsuarioAltaId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockActual_Producto_ProductoId",
                table: "StockActual",
                column: "ProductoId",
                principalTable: "Producto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
