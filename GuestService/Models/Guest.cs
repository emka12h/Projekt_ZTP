namespace GuestService.Models
{
    public class Guest
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string RsvpStatus { get; set; } = "Oczekujący";
        public string Group { get; set; } = string.Empty;
    }
}
