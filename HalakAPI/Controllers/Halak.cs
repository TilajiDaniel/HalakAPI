using HalakAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace HalakAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Halak : ControllerBase
    {
        public readonly HalakAPI.Models.HalakContext _context;
        public Halak(HalakAPI.Models.HalakContext context)
        {
            _context = context;
        }

        [HttpGet("FajMeretTo")]
        public async Task<ActionResult> GetFajMeretTo()
        {
            try
            {
                var lista = await _context.Halaks
                    .Include(h => h.To)
                    .Select(h => new HalFajMeretToDTO
                    {
                        Faj = h.Faj,
                        MeretCm = h.MeretCm,
                        ToNev = h.To != null ? h.To.Nev : "Nincs"
                    }).ToListAsync();

                return Ok(lista); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }
        [HttpPost]
        public IActionResult Add(HalakAPI.Models.Halak halak)
        {
            if (halak == null)
                return BadRequest("Üres objektum nem rögzíthetõ!");
            try
            {
                _context.Halaks.Add(halak);
                _context.SaveChanges();
                
                return Ok("Sikeres rögzítés.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public async Task<IActionResult> Update(HalakAPI.Models.Halak halak)
        {
            try
            {
                var letezik = await _context.Halaks.AnyAsync(h => h.Id == halak.Id);
                if (!letezik)
                {
                    return NotFound("Nincs ilyen azonosítójú hal!"); // 404 Not Found [cite: 30]
                }

                _context.Entry(halak).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(halak);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var halak = _context.Halaks.Where(x => x.Id == id).FirstOrDefault();
                if (halak.Id == null)
                    return BadRequest("Nincs ilyen azonosítójú hal!");

                _context.Halaks.Remove(halak);
                await _context.SaveChangesAsync();
                return Ok(halak);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        }
}
