using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace smartStock.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DireccionComoOwned : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "EstaActivo",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Dni",
                table: "AspNetUsers",
                type: "nchar(8)",
                fixedLength: true,
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Direccion_Calle",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Direccion_CodigoPostal",
                table: "AspNetUsers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Direccion_Localidad",
                table: "AspNetUsers",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Direccion_Numero",
                table: "AspNetUsers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Direccion_Pais",
                table: "AspNetUsers",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Direccion_Provincia",
                table: "AspNetUsers",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proveedor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cuit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SesionVenta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaSesion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SesionVenta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SesionVenta_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Producto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrecioCosto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioVenta = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    UsuarioAltaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Producto_AspNetUsers_UsuarioAltaId",
                        column: x => x.UsuarioAltaId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Producto_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SesionCompra",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaSesion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    ProveedorId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SesionCompra", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SesionCompra_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SesionCompra_Proveedor_ProveedorId",
                        column: x => x.ProveedorId,
                        principalTable: "Proveedor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransaccionVenta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SesionVentaId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransaccionVenta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransaccionVenta_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransaccionVenta_SesionVenta_SesionVentaId",
                        column: x => x.SesionVentaId,
                        principalTable: "SesionVenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockActual",
                columns: table => new
                {
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    UltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockActual", x => x.ProductoId);
                    table.ForeignKey(
                        name: "FK_StockActual_Producto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CierreCaja",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCierre = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalVentas = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCompras = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SaldoFinal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SesionVentaId = table.Column<int>(type: "int", nullable: true),
                    SesionCompraId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CierreCaja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CierreCaja_SesionCompra_SesionCompraId",
                        column: x => x.SesionCompraId,
                        principalTable: "SesionCompra",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CierreCaja_SesionVenta_SesionVentaId",
                        column: x => x.SesionVentaId,
                        principalTable: "SesionVenta",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TransaccionCompra",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SesionCompraId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransaccionCompra", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransaccionCompra_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransaccionCompra_SesionCompra_SesionCompraId",
                        column: x => x.SesionCompraId,
                        principalTable: "SesionCompra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemTransaccionVenta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cantidad = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioVenta = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioCosto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GananciaTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransaccionId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTransaccionVenta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemTransaccionVenta_Producto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemTransaccionVenta_TransaccionVenta_TransaccionId",
                        column: x => x.TransaccionId,
                        principalTable: "TransaccionVenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemTransaccionCompra",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cantidad = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioCompra = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransaccionId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTransaccionCompra", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemTransaccionCompra_Producto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemTransaccionCompra_TransaccionCompra_TransaccionId",
                        column: x => x.TransaccionId,
                        principalTable: "TransaccionCompra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovimientoStock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemVentaId = table.Column<int>(type: "int", nullable: true),
                    ItemCompraId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientoStock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovimientoStock_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovimientoStock_ItemTransaccionCompra_ItemCompraId",
                        column: x => x.ItemCompraId,
                        principalTable: "ItemTransaccionCompra",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MovimientoStock_ItemTransaccionVenta_ItemVentaId",
                        column: x => x.ItemVentaId,
                        principalTable: "ItemTransaccionVenta",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MovimientoStock_Producto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Dni",
                table: "AspNetUsers",
                column: "Dni",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_Nombre",
                table: "Categorias",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CierreCaja_SesionCompraId",
                table: "CierreCaja",
                column: "SesionCompraId",
                unique: true,
                filter: "[SesionCompraId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CierreCaja_SesionVentaId",
                table: "CierreCaja",
                column: "SesionVentaId",
                unique: true,
                filter: "[SesionVentaId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTransaccionCompra_ProductoId",
                table: "ItemTransaccionCompra",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTransaccionCompra_TransaccionId",
                table: "ItemTransaccionCompra",
                column: "TransaccionId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTransaccionVenta_ProductoId",
                table: "ItemTransaccionVenta",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTransaccionVenta_TransaccionId",
                table: "ItemTransaccionVenta",
                column: "TransaccionId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoStock_ItemCompraId",
                table: "MovimientoStock",
                column: "ItemCompraId",
                unique: true,
                filter: "[ItemCompraId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoStock_ItemVentaId",
                table: "MovimientoStock",
                column: "ItemVentaId",
                unique: true,
                filter: "[ItemVentaId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoStock_ProductoId",
                table: "MovimientoStock",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoStock_UsuarioId",
                table: "MovimientoStock",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_CategoriaId",
                table: "Producto",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_UsuarioAltaId",
                table: "Producto",
                column: "UsuarioAltaId");

            migrationBuilder.CreateIndex(
                name: "IX_SesionCompra_ProveedorId",
                table: "SesionCompra",
                column: "ProveedorId");

            migrationBuilder.CreateIndex(
                name: "IX_SesionCompra_UsuarioId",
                table: "SesionCompra",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_SesionVenta_UsuarioId",
                table: "SesionVenta",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TransaccionCompra_SesionCompraId",
                table: "TransaccionCompra",
                column: "SesionCompraId");

            migrationBuilder.CreateIndex(
                name: "IX_TransaccionCompra_UsuarioId",
                table: "TransaccionCompra",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TransaccionVenta_SesionVentaId",
                table: "TransaccionVenta",
                column: "SesionVentaId");

            migrationBuilder.CreateIndex(
                name: "IX_TransaccionVenta_UsuarioId",
                table: "TransaccionVenta",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CierreCaja");

            migrationBuilder.DropTable(
                name: "MovimientoStock");

            migrationBuilder.DropTable(
                name: "StockActual");

            migrationBuilder.DropTable(
                name: "ItemTransaccionCompra");

            migrationBuilder.DropTable(
                name: "ItemTransaccionVenta");

            migrationBuilder.DropTable(
                name: "TransaccionCompra");

            migrationBuilder.DropTable(
                name: "Producto");

            migrationBuilder.DropTable(
                name: "TransaccionVenta");

            migrationBuilder.DropTable(
                name: "SesionCompra");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "SesionVenta");

            migrationBuilder.DropTable(
                name: "Proveedor");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Dni",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Direccion_Calle",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Direccion_CodigoPostal",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Direccion_Localidad",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Direccion_Numero",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Direccion_Pais",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Direccion_Provincia",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<bool>(
                name: "EstaActivo",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "Dni",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nchar(8)",
                oldFixedLength: true,
                oldMaxLength: 8);

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
