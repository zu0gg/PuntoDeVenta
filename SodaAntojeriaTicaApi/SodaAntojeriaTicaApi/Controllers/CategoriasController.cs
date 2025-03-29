using Microsoft.AspNetCore.Mvc;
using SodaAntojeriaTicaApi.Models;
using SodaAntojeriaTicaApi.Repositories;

namespace SodaAntojeriaTicaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaRepository _repo;

        public CategoriaController(ICategoriaRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categorias = await _repo.ListarCategorias();
            return Ok(categorias);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var categoria = await _repo.ObtenerCategoriaPorId(id);
            if (categoria == null)
                return NotFound();
            return Ok(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoriaModel categoria)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var nuevoId = await _repo.InsertarCategoria(categoria);
            categoria.Id = nuevoId;

            return CreatedAtAction(nameof(Get), new { id = nuevoId }, categoria);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CategoriaModel categoria)
        {
            if (id != categoria.Id)
                return BadRequest("El ID no coincide.");

            await _repo.ActualizarCategoria(categoria);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.EliminarCategoria(id);
            return NoContent();
        }
    }
}

