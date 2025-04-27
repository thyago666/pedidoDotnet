using Microsoft.EntityFrameworkCore;
using PedidosLanche.Models;
using System.Collections.Generic;

namespace PedidosLanche.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Produto> Produtos { get; set; }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }

        public DbSet<ItemPedido> ItemPedido { get; set; }
    }
}
