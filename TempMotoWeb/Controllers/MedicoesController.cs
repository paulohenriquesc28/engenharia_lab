using Microsoft.AspNetCore.Mvc;
using TempMotoWeb.Data;
using TempMotoWeb.Models;


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

        // DELETE api/<ApiController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
