using GuestService.Data;
using GuestService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GuestService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        private readonly GuestDbContext _context;

        public GuestController(GuestDbContext context)
        {
            _context = context;
        }

        //ENDPOINT: Pobieranie listy wszystkich gości (GET)
        [HttpGet]
        public async Task<IActionResult> GetGuests()
        {
            var guests = await _context.Guests.ToListAsync();
            return Ok(guests);
        }

        //ENDPOINT: Dodawanie nowego gościa (POST)
        [HttpPost]
        public async Task<IActionResult> AddGuest([FromBody] Guest newGuest)
        {
            _context.Guests.Add(newGuest);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGuests), new { id = newGuest.Id }, newGuest);

        }

        //ENDPOINT: Zmianna statusu RSVP (PATCH)
        [HttpPatch("{id}/rsvp")]
        public async Task<IActionResult> UpdateRsvpStatus(int id, [FromBody] string rspvStatus)
        {
            var guest = await _context.Guests.FindAsync(id);
            if (guest == null)
            {
                return NotFound("Nie znaleziono gościa o podanym ID.");
            }
            guest.RsvpStatus = rspvStatus;
            await _context.SaveChangesAsync();
            return Ok("Status RSVP został zaktualizowany.");
        }
    }
}
