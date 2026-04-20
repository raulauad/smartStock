using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace smartStock.Api.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TokensRevocados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Jti = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Expiracion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokensRevocados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ContrasenaHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Dni = table.Column<string>(type: "nchar(8)", fixedLength: true, maxLength: 8, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Direccion_Pais = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Direccion_Provincia = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Direccion_Localidad = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Direccion_CodigoPostal = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Direccion_Calle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Direccion_Numero = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaBaja = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EstaActivo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Producto",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PrecioCosto = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    PrecioVenta = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    CategoriaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioAltaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Producto_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Producto_Usuarios_UsuarioAltaId",
                        column: x => x.UsuarioAltaId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Proveedores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Cuit = table.Column<string>(type: "nchar(11)", fixedLength: true, maxLength: 11, nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Direccion_Pais = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Direccion_Provincia = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Direccion_Localidad = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Direccion_CodigoPostal = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Direccion_Calle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Direccion_Numero = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EstaActivo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Proveedores_Usuarios_UsuarioAltaId",
                        column: x => x.UsuarioAltaId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioRoles",
                columns: table => new
                {
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioRoles", x => new { x.UsuarioId, x.Rol });
                    table.ForeignKey(
                        name: "FK_UsuarioRoles_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VentaDia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaSesion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VentaDia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VentaDia_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockActual",
                columns: table => new
                {
                    ProductoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                name: "CompraDia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaSesion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    ProveedorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompraDia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompraDia_Proveedores_ProveedorId",
                        column: x => x.ProveedorId,
                        principalTable: "Proveedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompraDia_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetalleVenta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VentaDiaId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleVenta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetalleVenta_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DetalleVenta_VentaDia_VentaDiaId",
                        column: x => x.VentaDiaId,
                        principalTable: "VentaDia",
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
                    TotalVentas = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    TotalCompras = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    SaldoFinal = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    VentaDiaId = table.Column<int>(type: "int", nullable: true),
                    CompraDiaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CierreCaja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CierreCaja_CompraDia_CompraDiaId",
                        column: x => x.CompraDiaId,
                        principalTable: "CompraDia",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CierreCaja_VentaDia_VentaDiaId",
                        column: x => x.VentaDiaId,
                        principalTable: "VentaDia",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DetalleCompra",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompraDiaId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleCompra", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetalleCompra_CompraDia_CompraDiaId",
                        column: x => x.CompraDiaId,
                        principalTable: "CompraDia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalleCompra_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ItemDetalleVenta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cantidad = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    PrecioVenta = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    PrecioCosto = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    GananciaTotal = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    DetalleVentaId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemDetalleVenta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemDetalleVenta_DetalleVenta_DetalleVentaId",
                        column: x => x.DetalleVentaId,
                        principalTable: "DetalleVenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemDetalleVenta_Producto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ItemDetalleCompra",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cantidad = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    PrecioCompra = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    DetalleCompraId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemDetalleCompra", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemDetalleCompra_DetalleCompra_DetalleCompraId",
                        column: x => x.DetalleCompraId,
                        principalTable: "DetalleCompra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemDetalleCompra_Producto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MovimientoStock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemVentaId = table.Column<int>(type: "int", nullable: true),
                    ItemCompraId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientoStock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovimientoStock_ItemDetalleCompra_ItemCompraId",
                        column: x => x.ItemCompraId,
                        principalTable: "ItemDetalleCompra",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MovimientoStock_ItemDetalleVenta_ItemVentaId",
                        column: x => x.ItemVentaId,
                        principalTable: "ItemDetalleVenta",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MovimientoStock_Producto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovimientoStock_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_Nombre",
                table: "Categorias",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CierreCaja_CompraDiaId",
                table: "CierreCaja",
                column: "CompraDiaId",
                unique: true,
                filter: "[CompraDiaId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CierreCaja_VentaDiaId",
                table: "CierreCaja",
                column: "VentaDiaId",
                unique: true,
                filter: "[VentaDiaId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CompraDia_ProveedorId",
                table: "CompraDia",
                column: "ProveedorId");

            migrationBuilder.CreateIndex(
                name: "IX_CompraDia_UsuarioId",
                table: "CompraDia",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleCompra_CompraDiaId",
                table: "DetalleCompra",
                column: "CompraDiaId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleCompra_UsuarioId",
                table: "DetalleCompra",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleVenta_UsuarioId",
                table: "DetalleVenta",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleVenta_VentaDiaId",
                table: "DetalleVenta",
                column: "VentaDiaId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDetalleCompra_DetalleCompraId",
                table: "ItemDetalleCompra",
                column: "DetalleCompraId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDetalleCompra_ProductoId",
                table: "ItemDetalleCompra",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDetalleVenta_DetalleVentaId",
                table: "ItemDetalleVenta",
                column: "DetalleVentaId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDetalleVenta_ProductoId",
                table: "ItemDetalleVenta",
                column: "ProductoId");

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
                name: "IX_Proveedores_Cuit",
                table: "Proveedores",
                column: "Cuit",
                unique: true,
                filter: "[Cuit] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_UsuarioAltaId",
                table: "Proveedores",
                column: "UsuarioAltaId");

            migrationBuilder.CreateIndex(
                name: "IX_TokensRevocados_Jti",
                table: "TokensRevocados",
                column: "Jti",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_UsuarioRoles_Administrador",
                table: "UsuarioRoles",
                column: "Rol",
                unique: true,
                filter: "[Rol] = 'Administrador'");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Dni",
                table: "Usuarios",
                column: "Dni",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VentaDia_UsuarioId",
                table: "VentaDia",
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
                name: "TokensRevocados");

            migrationBuilder.DropTable(
                name: "UsuarioRoles");

            migrationBuilder.DropTable(
                name: "ItemDetalleCompra");

            migrationBuilder.DropTable(
                name: "ItemDetalleVenta");

            migrationBuilder.DropTable(
                name: "DetalleCompra");

            migrationBuilder.DropTable(
                name: "DetalleVenta");

            migrationBuilder.DropTable(
                name: "Producto");

            migrationBuilder.DropTable(
                name: "CompraDia");

            migrationBuilder.DropTable(
                name: "VentaDia");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Proveedores");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
