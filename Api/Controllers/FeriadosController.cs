using Microsoft.AspNetCore.Mvc;
using FeriadosNacionaisAPI.Api.Models;
using FeriadosNacionaisAPI.Api.Services.Interfaces;

namespace FeriadosNacionaisAPI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeriadosController : ControllerBase
    {
        private readonly IFeriadoService _feriadoService;

        public FeriadosController(IFeriadoService feriadoService)
        {
            _feriadoService = feriadoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Feriado>>> GetFeriadosAsync()
        {
            var feriados = await _feriadoService.ObterFeriadosAsync();
            return Ok(feriados);
        }
    }
}