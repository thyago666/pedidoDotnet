using Microsoft.AspNetCore.Mvc.Rendering;

namespace PedidosLanche.Models
{
    public class PedidosPorClienteViewModel
    {
        public int? ClienteId { get; set; }

        public List<SelectListItem> Clientes { get; set; }

        public List<Pedido> Pedidos { get; set; }
    }
}
