using Microsoft.AspNetCore.Mvc.Rendering;

namespace PedidosLanche.Models
{
    public class PedidoViewModel
    {
        public int ClienteId { get; set; }
        public List<SelectListItem> Clientes { get; set; }

        public List<SelectListItem> Produtos { get; set; }

        public List<ItemPedidoViewModel> Itens { get; set; } = new List<ItemPedidoViewModel>();
    }
}
