using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PedidosLanche.Data;
using PedidosLanche.Models;

namespace PedidosLanche.Controllers
{
    [Authorize]
    public class CostumerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CostumerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            var clientes = _context.Clientes
    .Select(c => new Cliente
    {
        ClienteId = c.ClienteId,
        Nome = c.Nome,
        Telefone = c.Telefone,
       
    })
    .ToList();

            return View(clientes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente clientes)
        {
            if (ModelState.IsValid)
            {
                _context.Clientes.Add(clientes);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(clientes);
        }

        public IActionResult Edit(int id)
        {
            var clientes = _context.Clientes.Find(id);
            if (clientes == null) return NotFound();
            return View(clientes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Clientes.Update(cliente);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }
        public IActionResult Delete(int id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Cliente cliente)
        {
            _context.Clientes.Remove(cliente);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

    }


}

