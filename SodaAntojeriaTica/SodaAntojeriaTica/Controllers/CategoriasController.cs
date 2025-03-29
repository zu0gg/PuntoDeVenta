using Microsoft.AspNetCore.Mvc;
using SodaAntojeriaTica.Models;
using SodaAntojeriaTica.Services;

namespace SodaAntojeriaTica.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly CategoriaService _categoriaService;

        public CategoriasController(CategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        public async Task<IActionResult> Index()
        {
            var categorias = await _categoriaService.GetCategoriasAsync();
            return View(categorias);
        }

        public async Task<IActionResult> Details(int id)
        {
            var categoria = await _categoriaService.GetCategoriaByIdAsync(id);
            if (categoria == null)
                return NotFound();
            return View(categoria);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoriaModel categoria)
        {
            if (ModelState.IsValid)
            {
                await _categoriaService.CreateCategoriaAsync(categoria);
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var categoria = await _categoriaService.GetCategoriaByIdAsync(id);
            if (categoria == null)
                return NotFound();
            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoriaModel categoria)
        {
            if (id != categoria.Id)
                return BadRequest("El ID no coincide.");
            if (ModelState.IsValid)
            {
                await _categoriaService.UpdateCategoriaAsync(id, categoria);
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var categoria = await _categoriaService.GetCategoriaByIdAsync(id);
            if (categoria == null)
                return NotFound();
            return View(categoria);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoriaService.DeleteCategoriaAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
