using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidosLanche.Models
{
    public class Produto
    {

        public int ProdutoId { get; set; }
        public string Nome { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser maior que zero.")]
        public decimal Valor { get; set; }
        public string? ImagemUrl { get; set; }
        // Propriedade para receber o arquivo de imagem
        [NotMapped]
        public IFormFile Imagem { get; set; }
        public DateTime DataDeCriacao { get; set; } = DateTime.Now.ToLocalTime();
        public DateTime DataDeAlteracao { get; set; } = DateTime.Now.ToLocalTime();

    }
}
