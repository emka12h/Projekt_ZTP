using GuestService.Data;
using GuestService.Models;
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

        // GET: api/guest
        [HttpGet]
        public async Task<IActionResult> GetGuests(
            [FromQuery] Guid? weddingId,
            [FromQuery] RsvpStatus? rsvpStatus)
        {
            var query = _context.Guests.AsQueryable();

            // Filtrowanie po WeddingId
            if (weddingId.HasValue)
            {
                query = query.Where(g => g.WeddingId == weddingId.Value);
            }

            // Filtrowanie po RSVP
            if (rsvpStatus.HasValue)
            {
                query = query.Where(g => g.RsvpStatus == rsvpStatus.Value);
            }

            var guests = await query
                .OrderBy(g => g.LastName)
                .ThenBy(g => g.FirstName)
                .ToListAsync();

            return Ok(guests);
        }

        // GET: api/guest/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetGuestById(Guid id)
        {
            var guest = await _context.Guests.FindAsync(id);

            if (guest == null)
            {
                return NotFound("Nie znaleziono gościa.");
            }

            return Ok(guest);
        }

        // POST: api/guest
        [HttpPost]
        public async Task<IActionResult> AddGuest([FromBody] Guest newGuest)
        {
            // Generowanie GUID jeśli nie istnieje
            if (newGuest.Id == Guid.Empty)
            {
                newGuest.Id = Guid.NewGuid();
            }

            newGuest.CreatedAt = DateTime.UtcNow;
            newGuest.UpdatedAt = DateTime.UtcNow;

            _context.Guests.Add(newGuest);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetGuestById),
                new { id = newGuest.Id },
                newGuest
            );
        }

        // PUT: api/guest/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateGuest(Guid id, [FromBody] Guest updatedGuest)
        {
            var existingGuest = await _context.Guests.FindAsync(id);

            if (existingGuest == null)
            {
                return NotFound("Nie znaleziono gościa.");
            }

            // Aktualizacja pól
            existingGuest.FirstName = updatedGuest.FirstName;
            existingGuest.LastName = updatedGuest.LastName;
            existingGuest.Email = updatedGuest.Email;
            existingGuest.PhoneNumber = updatedGuest.PhoneNumber;
            existingGuest.Group = updatedGuest.Group;
            existingGuest.Side = updatedGuest.Side;
            existingGuest.IsVip = updatedGuest.IsVip;
            existingGuest.HasPlusOne = updatedGuest.HasPlusOne;
            existingGuest.PlusOneName = updatedGuest.PlusOneName;
            existingGuest.TableNumber = updatedGuest.TableNumber;
            existingGuest.SeatNumber = updatedGuest.SeatNumber;
            existingGuest.MealPreference = updatedGuest.MealPreference;
            existingGuest.Allergies = updatedGuest.Allergies;
            existingGuest.NeedsAccommodation = updatedGuest.NeedsAccommodation;
            existingGuest.NeedsTransport = updatedGuest.NeedsTransport;
            existingGuest.Notes = updatedGuest.Notes;

            existingGuest.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(existingGuest);
        }

        // PATCH: api/guest/{id}/rsvp
        [HttpPatch("{id:guid}/rsvp")]
        public async Task<IActionResult> UpdateRsvpStatus(
            Guid id,
            [FromBody] RsvpStatus rsvpStatus)
        {
            var guest = await _context.Guests.FindAsync(id);

            if (guest == null)
            {
                return NotFound("Nie znaleziono gościa o podanym ID.");
            }

            guest.RsvpStatus = rsvpStatus;
            guest.UpdatedAt = DateTime.UtcNow;

            // Automatyczny timestamp odpowiedzi RSVP
            if (rsvpStatus == RsvpStatus.Confirmed ||
                rsvpStatus == RsvpStatus.Declined)
            {
                guest.RsvpRespondedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Status RSVP został zaktualizowany.",
                GuestId = guest.Id,
                NewStatus = guest.RsvpStatus
            });
        }

        // PATCH: api/guest/{id}/checkin
        [HttpPatch("{id:guid}/checkin")]
        public async Task<IActionResult> CheckInGuest(Guid id)
        {
            var guest = await _context.Guests.FindAsync(id);

            if (guest == null)
            {
                return NotFound("Nie znaleziono gościa.");
            }

            guest.CheckedIn = true;
            guest.CheckedInAt = DateTime.UtcNow;
            guest.RsvpStatus = RsvpStatus.CheckedIn;
            guest.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Gość został zameldowany.",
                GuestId = guest.Id
            });
        }

        // DELETE: api/guest/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteGuest(Guid id)
        {
            var guest = await _context.Guests.FindAsync(id);

            if (guest == null)
            {
                return NotFound("Nie znaleziono gościa.");
            }

            _context.Guests.Remove(guest);
            await _context.SaveChangesAsync();

            return Ok("Gość został usunięty.");
        }
    }
}