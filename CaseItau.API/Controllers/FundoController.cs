using Application.Interfaces;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaseItau.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FundoController : ControllerBase
    {
        private readonly IFundoService _fundoService;
        public FundoController(IFundoService fundoService)
        {
            _fundoService = fundoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        => Ok(await _fundoService.BuscarTodosAsync());

        [HttpGet("{codigo}")]
        public async Task<IActionResult> GetAsync(string codigo)
        {
            var fundo = await _fundoService.BuscarPorCodigoAsync(codigo);
            return fundo is null ? NotFound() : Ok(fundo);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CriarFundoRequest request)
        {
            var fundo = await _fundoService.CriarFundoAsync(request);
            return Created($"/api/fundo/{fundo.Codigo}", fundo);
        }

        [HttpPut("{codigo}")]
        public async Task<IActionResult> PutAsync(string codigo, [FromBody] AtualizarFundoRequest request)
        {
            var existe = await _fundoService.BuscarPorCodigoAsync(codigo);
            if (existe is null) return NotFound();

            await _fundoService.AtualizarFundoAsync(codigo, request);
            return NoContent();
        }

        [HttpDelete("{codigo}")]
        public async Task<IActionResult> DeleteAsync(string codigo)
        {
            var fundo = await _fundoService.BuscarPorCodigoAsync(codigo);
            if (fundo is null) return NotFound();
            await _fundoService.DeletarFundoAsync(codigo);
            return NoContent();
        }

        [HttpPut("{codigo}/patrimonio")]
        public async Task<IActionResult> MovimentarPatrimonioAsync(string codigo, [FromBody] decimal valor)
        {
            var fundo = await _fundoService.BuscarPorCodigoAsync(codigo);
            if (fundo is null) return NotFound();
            await _fundoService.MovimentarPatrimonioAsync(codigo, valor);
            return NoContent();
        }
    }
}
