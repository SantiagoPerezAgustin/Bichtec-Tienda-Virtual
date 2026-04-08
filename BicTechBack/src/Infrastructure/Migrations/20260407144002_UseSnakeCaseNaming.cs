using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UseSnakeCaseNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carritos_Usuarios_UsuarioId",
                table: "Carritos");

            migrationBuilder.DropForeignKey(
                name: "FK_CarritosDetalles_Carritos_CarritoId",
                table: "CarritosDetalles");

            migrationBuilder.DropForeignKey(
                name: "FK_CarritosDetalles_Productos_ProductoId",
                table: "CarritosDetalles");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoriasMarcas_Categorias_CategoriaId",
                table: "CategoriasMarcas");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoriasMarcas_Marcas_MarcaId",
                table: "CategoriasMarcas");

            migrationBuilder.DropForeignKey(
                name: "FK_Marcas_Paises_PaisId",
                table: "Marcas");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Usuarios_UsuarioId",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidosDetalles_Pedidos_PedidoId",
                table: "PedidosDetalles");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidosDetalles_Productos_ProductoId",
                table: "PedidosDetalles");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Categorias_CategoriaId",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Marcas_MarcaId",
                table: "Productos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuarios",
                table: "Usuarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Productos",
                table: "Productos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pedidos",
                table: "Pedidos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Paises",
                table: "Paises");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Marcas",
                table: "Marcas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categorias",
                table: "Categorias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Carritos",
                table: "Carritos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PedidosDetalles",
                table: "PedidosDetalles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoriasMarcas",
                table: "CategoriasMarcas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarritosDetalles",
                table: "CarritosDetalles");

            migrationBuilder.RenameTable(
                name: "Usuarios",
                newName: "usuarios");

            migrationBuilder.RenameTable(
                name: "Productos",
                newName: "productos");

            migrationBuilder.RenameTable(
                name: "Pedidos",
                newName: "pedidos");

            migrationBuilder.RenameTable(
                name: "Paises",
                newName: "paises");

            migrationBuilder.RenameTable(
                name: "Marcas",
                newName: "marcas");

            migrationBuilder.RenameTable(
                name: "Categorias",
                newName: "categorias");

            migrationBuilder.RenameTable(
                name: "Carritos",
                newName: "carritos");

            migrationBuilder.RenameTable(
                name: "PedidosDetalles",
                newName: "pedidos_detalles");

            migrationBuilder.RenameTable(
                name: "CategoriasMarcas",
                newName: "categorias_marcas");

            migrationBuilder.RenameTable(
                name: "CarritosDetalles",
                newName: "carritos_detalles");

            migrationBuilder.RenameColumn(
                name: "Rol",
                table: "usuarios",
                newName: "rol");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "usuarios",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "usuarios",
                newName: "nombre");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "usuarios",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "usuarios",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "RefreshTokenExpiryTime",
                table: "usuarios",
                newName: "refresh_token_expiry_time");

            migrationBuilder.RenameColumn(
                name: "RefreshToken",
                table: "usuarios",
                newName: "refresh_token");

            migrationBuilder.RenameColumn(
                name: "Stock",
                table: "productos",
                newName: "stock");

            migrationBuilder.RenameColumn(
                name: "Precio",
                table: "productos",
                newName: "precio");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "productos",
                newName: "nombre");

            migrationBuilder.RenameColumn(
                name: "Descripcion",
                table: "productos",
                newName: "descripcion");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "productos",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "MarcaId",
                table: "productos",
                newName: "marca_id");

            migrationBuilder.RenameColumn(
                name: "ImagenUrl",
                table: "productos",
                newName: "imagen_url");

            migrationBuilder.RenameColumn(
                name: "CategoriaId",
                table: "productos",
                newName: "categoria_id");

            migrationBuilder.RenameIndex(
                name: "IX_Productos_MarcaId",
                table: "productos",
                newName: "ix_productos_marca_id");

            migrationBuilder.RenameIndex(
                name: "IX_Productos_CategoriaId",
                table: "productos",
                newName: "ix_productos_categoria_id");

            migrationBuilder.RenameColumn(
                name: "Total",
                table: "pedidos",
                newName: "total");

            migrationBuilder.RenameColumn(
                name: "Estado",
                table: "pedidos",
                newName: "estado");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "pedidos",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "pedidos",
                newName: "usuario_id");

            migrationBuilder.RenameColumn(
                name: "FechaPedido",
                table: "pedidos",
                newName: "fecha_pedido");

            migrationBuilder.RenameColumn(
                name: "DireccionEnvio",
                table: "pedidos",
                newName: "direccion_envio");

            migrationBuilder.RenameIndex(
                name: "IX_Pedidos_UsuarioId",
                table: "pedidos",
                newName: "ix_pedidos_usuario_id");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "paises",
                newName: "nombre");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "paises",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "marcas",
                newName: "nombre");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "marcas",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "PaisId",
                table: "marcas",
                newName: "pais_id");

            migrationBuilder.RenameIndex(
                name: "IX_Marcas_PaisId",
                table: "marcas",
                newName: "ix_marcas_pais_id");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "categorias",
                newName: "nombre");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "categorias",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "carritos",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "carritos",
                newName: "usuario_id");

            migrationBuilder.RenameColumn(
                name: "ActualizadoEn",
                table: "carritos",
                newName: "actualizado_en");

            migrationBuilder.RenameIndex(
                name: "IX_Carritos_UsuarioId",
                table: "carritos",
                newName: "ix_carritos_usuario_id");

            migrationBuilder.RenameColumn(
                name: "Subtotal",
                table: "pedidos_detalles",
                newName: "subtotal");

            migrationBuilder.RenameColumn(
                name: "Precio",
                table: "pedidos_detalles",
                newName: "precio");

            migrationBuilder.RenameColumn(
                name: "Cantidad",
                table: "pedidos_detalles",
                newName: "cantidad");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "pedidos_detalles",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ProductoId",
                table: "pedidos_detalles",
                newName: "producto_id");

            migrationBuilder.RenameColumn(
                name: "PedidoId",
                table: "pedidos_detalles",
                newName: "pedido_id");

            migrationBuilder.RenameIndex(
                name: "IX_PedidosDetalles_ProductoId",
                table: "pedidos_detalles",
                newName: "ix_pedidos_detalles_producto_id");

            migrationBuilder.RenameIndex(
                name: "IX_PedidosDetalles_PedidoId",
                table: "pedidos_detalles",
                newName: "ix_pedidos_detalles_pedido_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "categorias_marcas",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "MarcaId",
                table: "categorias_marcas",
                newName: "marca_id");

            migrationBuilder.RenameColumn(
                name: "CategoriaId",
                table: "categorias_marcas",
                newName: "categoria_id");

            migrationBuilder.RenameIndex(
                name: "IX_CategoriasMarcas_MarcaId",
                table: "categorias_marcas",
                newName: "ix_categorias_marcas_marca_id");

            migrationBuilder.RenameIndex(
                name: "IX_CategoriasMarcas_CategoriaId",
                table: "categorias_marcas",
                newName: "ix_categorias_marcas_categoria_id");

            migrationBuilder.RenameColumn(
                name: "Cantidad",
                table: "carritos_detalles",
                newName: "cantidad");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "carritos_detalles",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ProductoId",
                table: "carritos_detalles",
                newName: "producto_id");

            migrationBuilder.RenameColumn(
                name: "CarritoId",
                table: "carritos_detalles",
                newName: "carrito_id");

            migrationBuilder.RenameIndex(
                name: "IX_CarritosDetalles_ProductoId",
                table: "carritos_detalles",
                newName: "ix_carritos_detalles_producto_id");

            migrationBuilder.RenameIndex(
                name: "IX_CarritosDetalles_CarritoId",
                table: "carritos_detalles",
                newName: "ix_carritos_detalles_carrito_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_usuarios",
                table: "usuarios",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_productos",
                table: "productos",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_pedidos",
                table: "pedidos",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_paises",
                table: "paises",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_marcas",
                table: "marcas",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_categorias",
                table: "categorias",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_carritos",
                table: "carritos",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_pedidos_detalles",
                table: "pedidos_detalles",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_categorias_marcas",
                table: "categorias_marcas",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_carritos_detalles",
                table: "carritos_detalles",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_carritos_usuarios_usuario_id",
                table: "carritos",
                column: "usuario_id",
                principalTable: "usuarios",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_carritos_detalles_carritos_carrito_id",
                table: "carritos_detalles",
                column: "carrito_id",
                principalTable: "carritos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_carritos_detalles_productos_producto_id",
                table: "carritos_detalles",
                column: "producto_id",
                principalTable: "productos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_categorias_marcas_categorias_categoria_id",
                table: "categorias_marcas",
                column: "categoria_id",
                principalTable: "categorias",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_categorias_marcas_marcas_marca_id",
                table: "categorias_marcas",
                column: "marca_id",
                principalTable: "marcas",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_marcas_paises_pais_id",
                table: "marcas",
                column: "pais_id",
                principalTable: "paises",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_pedidos_usuarios_usuario_id",
                table: "pedidos",
                column: "usuario_id",
                principalTable: "usuarios",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_pedidos_detalles_pedidos_pedido_id",
                table: "pedidos_detalles",
                column: "pedido_id",
                principalTable: "pedidos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_pedidos_detalles_productos_producto_id",
                table: "pedidos_detalles",
                column: "producto_id",
                principalTable: "productos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_productos_categorias_categoria_id",
                table: "productos",
                column: "categoria_id",
                principalTable: "categorias",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_productos_marcas_marca_id",
                table: "productos",
                column: "marca_id",
                principalTable: "marcas",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_carritos_usuarios_usuario_id",
                table: "carritos");

            migrationBuilder.DropForeignKey(
                name: "fk_carritos_detalles_carritos_carrito_id",
                table: "carritos_detalles");

            migrationBuilder.DropForeignKey(
                name: "fk_carritos_detalles_productos_producto_id",
                table: "carritos_detalles");

            migrationBuilder.DropForeignKey(
                name: "fk_categorias_marcas_categorias_categoria_id",
                table: "categorias_marcas");

            migrationBuilder.DropForeignKey(
                name: "fk_categorias_marcas_marcas_marca_id",
                table: "categorias_marcas");

            migrationBuilder.DropForeignKey(
                name: "fk_marcas_paises_pais_id",
                table: "marcas");

            migrationBuilder.DropForeignKey(
                name: "fk_pedidos_usuarios_usuario_id",
                table: "pedidos");

            migrationBuilder.DropForeignKey(
                name: "fk_pedidos_detalles_pedidos_pedido_id",
                table: "pedidos_detalles");

            migrationBuilder.DropForeignKey(
                name: "fk_pedidos_detalles_productos_producto_id",
                table: "pedidos_detalles");

            migrationBuilder.DropForeignKey(
                name: "fk_productos_categorias_categoria_id",
                table: "productos");

            migrationBuilder.DropForeignKey(
                name: "fk_productos_marcas_marca_id",
                table: "productos");

            migrationBuilder.DropPrimaryKey(
                name: "pk_usuarios",
                table: "usuarios");

            migrationBuilder.DropPrimaryKey(
                name: "pk_productos",
                table: "productos");

            migrationBuilder.DropPrimaryKey(
                name: "pk_pedidos",
                table: "pedidos");

            migrationBuilder.DropPrimaryKey(
                name: "pk_paises",
                table: "paises");

            migrationBuilder.DropPrimaryKey(
                name: "pk_marcas",
                table: "marcas");

            migrationBuilder.DropPrimaryKey(
                name: "pk_categorias",
                table: "categorias");

            migrationBuilder.DropPrimaryKey(
                name: "pk_carritos",
                table: "carritos");

            migrationBuilder.DropPrimaryKey(
                name: "pk_pedidos_detalles",
                table: "pedidos_detalles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_categorias_marcas",
                table: "categorias_marcas");

            migrationBuilder.DropPrimaryKey(
                name: "pk_carritos_detalles",
                table: "carritos_detalles");

            migrationBuilder.RenameTable(
                name: "usuarios",
                newName: "Usuarios");

            migrationBuilder.RenameTable(
                name: "productos",
                newName: "Productos");

            migrationBuilder.RenameTable(
                name: "pedidos",
                newName: "Pedidos");

            migrationBuilder.RenameTable(
                name: "paises",
                newName: "Paises");

            migrationBuilder.RenameTable(
                name: "marcas",
                newName: "Marcas");

            migrationBuilder.RenameTable(
                name: "categorias",
                newName: "Categorias");

            migrationBuilder.RenameTable(
                name: "carritos",
                newName: "Carritos");

            migrationBuilder.RenameTable(
                name: "pedidos_detalles",
                newName: "PedidosDetalles");

            migrationBuilder.RenameTable(
                name: "categorias_marcas",
                newName: "CategoriasMarcas");

            migrationBuilder.RenameTable(
                name: "carritos_detalles",
                newName: "CarritosDetalles");

            migrationBuilder.RenameColumn(
                name: "rol",
                table: "Usuarios",
                newName: "Rol");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "Usuarios",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "nombre",
                table: "Usuarios",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Usuarios",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Usuarios",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "refresh_token_expiry_time",
                table: "Usuarios",
                newName: "RefreshTokenExpiryTime");

            migrationBuilder.RenameColumn(
                name: "refresh_token",
                table: "Usuarios",
                newName: "RefreshToken");

            migrationBuilder.RenameColumn(
                name: "stock",
                table: "Productos",
                newName: "Stock");

            migrationBuilder.RenameColumn(
                name: "precio",
                table: "Productos",
                newName: "Precio");

            migrationBuilder.RenameColumn(
                name: "nombre",
                table: "Productos",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "descripcion",
                table: "Productos",
                newName: "Descripcion");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Productos",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "marca_id",
                table: "Productos",
                newName: "MarcaId");

            migrationBuilder.RenameColumn(
                name: "imagen_url",
                table: "Productos",
                newName: "ImagenUrl");

            migrationBuilder.RenameColumn(
                name: "categoria_id",
                table: "Productos",
                newName: "CategoriaId");

            migrationBuilder.RenameIndex(
                name: "ix_productos_marca_id",
                table: "Productos",
                newName: "IX_Productos_MarcaId");

            migrationBuilder.RenameIndex(
                name: "ix_productos_categoria_id",
                table: "Productos",
                newName: "IX_Productos_CategoriaId");

            migrationBuilder.RenameColumn(
                name: "total",
                table: "Pedidos",
                newName: "Total");

            migrationBuilder.RenameColumn(
                name: "estado",
                table: "Pedidos",
                newName: "Estado");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Pedidos",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "usuario_id",
                table: "Pedidos",
                newName: "UsuarioId");

            migrationBuilder.RenameColumn(
                name: "fecha_pedido",
                table: "Pedidos",
                newName: "FechaPedido");

            migrationBuilder.RenameColumn(
                name: "direccion_envio",
                table: "Pedidos",
                newName: "DireccionEnvio");

            migrationBuilder.RenameIndex(
                name: "ix_pedidos_usuario_id",
                table: "Pedidos",
                newName: "IX_Pedidos_UsuarioId");

            migrationBuilder.RenameColumn(
                name: "nombre",
                table: "Paises",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Paises",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "nombre",
                table: "Marcas",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Marcas",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "pais_id",
                table: "Marcas",
                newName: "PaisId");

            migrationBuilder.RenameIndex(
                name: "ix_marcas_pais_id",
                table: "Marcas",
                newName: "IX_Marcas_PaisId");

            migrationBuilder.RenameColumn(
                name: "nombre",
                table: "Categorias",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Categorias",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Carritos",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "usuario_id",
                table: "Carritos",
                newName: "UsuarioId");

            migrationBuilder.RenameColumn(
                name: "actualizado_en",
                table: "Carritos",
                newName: "ActualizadoEn");

            migrationBuilder.RenameIndex(
                name: "ix_carritos_usuario_id",
                table: "Carritos",
                newName: "IX_Carritos_UsuarioId");

            migrationBuilder.RenameColumn(
                name: "subtotal",
                table: "PedidosDetalles",
                newName: "Subtotal");

            migrationBuilder.RenameColumn(
                name: "precio",
                table: "PedidosDetalles",
                newName: "Precio");

            migrationBuilder.RenameColumn(
                name: "cantidad",
                table: "PedidosDetalles",
                newName: "Cantidad");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "PedidosDetalles",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "producto_id",
                table: "PedidosDetalles",
                newName: "ProductoId");

            migrationBuilder.RenameColumn(
                name: "pedido_id",
                table: "PedidosDetalles",
                newName: "PedidoId");

            migrationBuilder.RenameIndex(
                name: "ix_pedidos_detalles_producto_id",
                table: "PedidosDetalles",
                newName: "IX_PedidosDetalles_ProductoId");

            migrationBuilder.RenameIndex(
                name: "ix_pedidos_detalles_pedido_id",
                table: "PedidosDetalles",
                newName: "IX_PedidosDetalles_PedidoId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "CategoriasMarcas",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "marca_id",
                table: "CategoriasMarcas",
                newName: "MarcaId");

            migrationBuilder.RenameColumn(
                name: "categoria_id",
                table: "CategoriasMarcas",
                newName: "CategoriaId");

            migrationBuilder.RenameIndex(
                name: "ix_categorias_marcas_marca_id",
                table: "CategoriasMarcas",
                newName: "IX_CategoriasMarcas_MarcaId");

            migrationBuilder.RenameIndex(
                name: "ix_categorias_marcas_categoria_id",
                table: "CategoriasMarcas",
                newName: "IX_CategoriasMarcas_CategoriaId");

            migrationBuilder.RenameColumn(
                name: "cantidad",
                table: "CarritosDetalles",
                newName: "Cantidad");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "CarritosDetalles",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "producto_id",
                table: "CarritosDetalles",
                newName: "ProductoId");

            migrationBuilder.RenameColumn(
                name: "carrito_id",
                table: "CarritosDetalles",
                newName: "CarritoId");

            migrationBuilder.RenameIndex(
                name: "ix_carritos_detalles_producto_id",
                table: "CarritosDetalles",
                newName: "IX_CarritosDetalles_ProductoId");

            migrationBuilder.RenameIndex(
                name: "ix_carritos_detalles_carrito_id",
                table: "CarritosDetalles",
                newName: "IX_CarritosDetalles_CarritoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuarios",
                table: "Usuarios",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Productos",
                table: "Productos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pedidos",
                table: "Pedidos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Paises",
                table: "Paises",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Marcas",
                table: "Marcas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categorias",
                table: "Categorias",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Carritos",
                table: "Carritos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PedidosDetalles",
                table: "PedidosDetalles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoriasMarcas",
                table: "CategoriasMarcas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarritosDetalles",
                table: "CarritosDetalles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Carritos_Usuarios_UsuarioId",
                table: "Carritos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarritosDetalles_Carritos_CarritoId",
                table: "CarritosDetalles",
                column: "CarritoId",
                principalTable: "Carritos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarritosDetalles_Productos_ProductoId",
                table: "CarritosDetalles",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoriasMarcas_Categorias_CategoriaId",
                table: "CategoriasMarcas",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoriasMarcas_Marcas_MarcaId",
                table: "CategoriasMarcas",
                column: "MarcaId",
                principalTable: "Marcas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Marcas_Paises_PaisId",
                table: "Marcas",
                column: "PaisId",
                principalTable: "Paises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Usuarios_UsuarioId",
                table: "Pedidos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidosDetalles_Pedidos_PedidoId",
                table: "PedidosDetalles",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidosDetalles_Productos_ProductoId",
                table: "PedidosDetalles",
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
                name: "FK_Productos_Marcas_MarcaId",
                table: "Productos",
                column: "MarcaId",
                principalTable: "Marcas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
