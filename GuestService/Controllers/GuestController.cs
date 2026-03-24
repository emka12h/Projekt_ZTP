using Microsoft.AspNetCore.Mvc;

namespace GuestService.Controllers
{
    [ApiController]
    [Route("api/guests")]
    public class GuestController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetGuests()
        {
            var guests = new[]
            {
                new { Id = 1, FirstName = "Jan", LastName = "Kowalski", Status = "Potwierdzony" },
                new { Id = 2, FirstName = "Anna", LastName = "Nowak", Status = "Brak odpowiedzi" }
            };
            return Ok(guests);
        }
    }
}