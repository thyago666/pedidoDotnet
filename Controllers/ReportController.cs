using PedidosLanche.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using PedidosLanche.Data;

public class ReportController : Controller
{
    private readonly ApplicationDbContext _context;

    public ReportController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult ProdutosMaisVendidos()
    {
        var dados = _context.ItemPedido
            .GroupBy(i => i.Produto.Nome)
            .Select(g => new
            {
                ProdutoNome = g.Key,
                QuantidadeTotal = g.Sum(i => i.Quantidade)
            })
            .OrderByDescending(x => x.QuantidadeTotal)
            .Take(10) // top 10
            .ToList();

        ViewBag.Labels = dados.Select(d => d.ProdutoNome).ToList();
        ViewBag.Dados = dados.Select(d => d.QuantidadeTotal).ToList();

        return View();
    }
}

