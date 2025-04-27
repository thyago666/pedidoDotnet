using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PedidosLanche.Data;
using PedidosLanche.Models;

namespace PedidosLanche.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            var model = new PedidoViewModel
            {
                Clientes = _context.Clientes.Select(c => new SelectListItem
                {
                    Value = c.ClienteId.ToString(),
                    Text = c.Nome
                }).ToList(),

                Produtos = _context.Produtos.Select(p => new SelectListItem
                {
                    Value = p.ProdutoId.ToString(),
                    Text = p.Nome
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(PedidoViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Recarrega os selects caso haja erro
                model.Clientes = _context.Clientes.Select(c => new SelectListItem
                {
                    Value = c.ClienteId.ToString(),
                    Text = c.Nome
                }).ToList();

                model.Produtos = _context.Produtos.Select(p => new SelectListItem
                {
                    Value = p.ProdutoId.ToString(),
                    Text = p.Nome
                }).ToList();

                return View(model);
            }

            var pedido = new Pedido
            {
                ClienteId = model.ClienteId,
                DataPedido = DateTime.Now,
                Itens = model.Itens.Select(i => new ItemPedido
                {
                    ProdutoId = i.ProdutoId,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = _context.Produtos.First(p => p.ProdutoId == i.ProdutoId).Valor
                }).ToList()
            };

            _context.Pedidos.Add(pedido);
            _context.SaveChanges();

            return RedirectToAction("Create");
        }

        public IActionResult ListaPorCliente(int? clienteId)
        {
            var clientes = _context.Clientes.Select(c => new SelectListItem
            {
                Value = c.ClienteId.ToString(),
                Text = c.Nome
            }).ToList();

            var pedidos = new List<Pedido>();

            if (clienteId.HasValue)
            {
                pedidos = _context.Pedidos
                    .Include(p => p.Itens)
                    .ThenInclude(i => i.Produto)
                    .Where(p => p.ClienteId == clienteId)
                    .ToList();
            }

            var viewModel = new PedidosPorClienteViewModel
            {
                ClienteId = clienteId,
                Clientes = clientes,
                Pedidos = pedidos
            };

            return View(viewModel);
        }
    }
}
