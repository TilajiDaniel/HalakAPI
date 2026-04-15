using HalakAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HalakAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Horgaszok : ControllerBase
    {
        public HalakAPI.Models.HalakContext _context;
        public Horgaszok(HalakAPI.Models.HalakContext context)
        {
            _context = context;
        }
        [HttpGet("All")]
        public IActionResult GetAll()
        {
            try
            {
                var horgasz = _context.Horgaszoks.ToList();

                return Ok(horgasz);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("ById/{id}")]
        public IActionResult GetById(int id)
        {
            try
            {

                var horgasz = _context.Horgaszoks.Where(x => x.Id == id).FirstOrDefault();
                if (horgasz == null)
                {
                    return NotFound("Nincs ilyen azonosítójú horgász!"); // 404 Not Found [cite: 23]
                }
                return Ok(horgasz);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        }
}
