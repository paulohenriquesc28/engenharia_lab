using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;
using System.Globalization;
using System.Net.Http.Headers;
using TempMotoWeb.Data;
using TempMotoWeb.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TempMotoWeb.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class MedicoesController : ControllerBase
    {
        private readonly AquaContext _context;

        public MedicoesController(AquaContext context)
        {
            _context = context;
        }

        // POST api/<ApiController>
        [HttpPost]
        public async Task<IResult> Post(Medicao medicao)
        {
            try
            {
                _context.Medicao.Add(medicao);
                await _context.SaveChangesAsync();

                return Results.Created($"/medicao/{medicao.Id}", medicao);
            }catch(Exception e)
            {
                return Results.Problem(e.Message+e.StackTrace);
            }
        }

        [HttpGet("mapa")]
        public async Task<List<Medicao>> Mapa([FromQuery]int[] itens)
        {
            var list = await _context.Medicao
                .Where(x => itens.Contains(x.Id))
                .ToListAsync();

            /*foreach(var it in list)
            {
                it.Data_Medicao = DateTime.Parse(it.Data_Medicao.ToString(), new CultureInfo("pt-BR"));
            }*/

            return list;
        }

        [HttpGet("grafico")]
        public async Task<List<Medicao>> Grafico([FromQuery] DateTime dataInicio, [FromQuery] DateTime dataFim)
        {
            var list = await _context.Medicao
                .Where(x => x.Data_Medicao>dataInicio && x.Data_Medicao < dataFim)
                .ToListAsync();

            /*foreach(var it in list)
            {
                it.Data_Medicao = DateTime.Parse(it.Data_Medicao.ToString(), new CultureInfo("pt-BR"));
            }*/

            return list;
        }

        // DELETE api/<ApiController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
