using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PedidosLanche.Data;
using PedidosLanche.Models;

namespace PedidosLanche.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            var produtos = _context.Produtos
    .Select(p => new Produto
    {
        ProdutoId = p.ProdutoId,
        Nome = p.Nome,
        Valor = p.Valor,
        ImagemUrl = string.IsNullOrEmpty(p.ImagemUrl) ? "sem-imagem.jpg" : p.ImagemUrl
    })
    .ToList();

            return View(produtos);
        }


        public IActionResult Create()
        {
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Produto produtos)
        {
            if (ModelState.IsValid)
            {
                // Verifica se foi enviado um arquivo
                if (produtos.Imagem != null && produtos.Imagem.Length > 0)
                {
                    // Define o caminho onde a imagem será salva (wwwroot/imagens)
                    var caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    // Cria o diretório, se não existir
                    if (!Directory.Exists(caminho))
                        Directory.CreateDirectory(caminho);

                    // Gera um nome único para o arquivo
                    var nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(produtos.Imagem.FileName);

                    var caminhoCompleto = Path.Combine(caminho, nomeArquivo);

                    using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                    {
                        await produtos.Imagem.CopyToAsync(stream);
                    }

                    // Salva o nome do arquivo no campo ImagemUrl
                    produtos.ImagemUrl = "/images/" + nomeArquivo;
                }

                _context.Produtos.Add(produtos);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(produtos);
        }

        public IActionResult Edit(int id)
        {
            var produto = _context.Produtos.Find(id);
            if (produto == null) return NotFound();
            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Produto produto)
        {
            if (!ModelState.IsValid)
            {
                _context.Produtos.Update(produto);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        public IActionResult Details(int id)
        {
            var produto = _context.Produtos.Find(id);
            if (produto == null) return NotFound();
            return View(produto);
        }

        public IActionResult Delete(int id)
        {
            var produto = _context.Produtos.Find(id);
            if (produto == null) return NotFound();
            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Produto produto)
        {
            _context.Produtos.Remove(produto);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AlterarImagem(int id, IFormFile imagem)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            if (imagem != null && imagem.Length > 0)
            {
                // Caminho onde você quer salvar (ex: wwwroot/imagens)
                var caminhoPasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagens");
                Directory.CreateDirectory(caminhoPasta); // Garante que a pasta exista

                var nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(imagem.FileName);
                var caminhoCompleto = Path.Combine(caminhoPasta, nomeArquivo);

                using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                {
                    await imagem.CopyToAsync(stream);
                }

                // Atualiza a URL da imagem
                produto.ImagemUrl = "/imagens/" + nomeArquivo;
                produto.DataDeAlteracao = DateTime.Now;

                _context.Produtos.Update(produto);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = produto.ProdutoId });
        }





    }
}
