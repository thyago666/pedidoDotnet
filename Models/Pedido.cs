namespace PedidosLanche.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public DateTime DataPedido { get; set; } = DateTime.Now.ToLocalTime();
        public List<ItemPedido> Itens { get; set; }
    }
}
