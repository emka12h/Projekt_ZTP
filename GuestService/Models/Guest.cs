namespace GuestService.Models
{
    public class Guest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid WeddingId { get; set; }

        // Dane podstawowe
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        // RSVP
        public RsvpStatus RsvpStatus { get; set; } = RsvpStatus.Pending;

        public DateTime? InvitationSentAt { get; set; }
        public DateTime? RsvpRespondedAt { get; set; }

        // Organizacja
        public GuestGroup Group { get; set; }
        public GuestSide Side { get; set; }

        public bool IsVip { get; set; }

        public bool HasPlusOne { get; set; }
        public string? PlusOneName { get; set; }

        // Rozsadzenie
        public int? TableNumber { get; set; }
        public string? SeatNumber { get; set; }

        // Preferencje
        public MealPreference MealPreference { get; set; }
        public string? Allergies { get; set; }

        // Logistyka
        public bool NeedsAccommodation { get; set; }
        public bool NeedsTransport { get; set; }

        // Check-in
        public bool CheckedIn { get; set; }
        public DateTime? CheckedInAt { get; set; }

        // Dodatkowe
        public string? Notes { get; set; }

        // Audit
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum RsvpStatus
    {
        Pending,
        Confirmed,
        Declined,
        CheckedIn
    }

    public enum GuestGroup
    {
        Family,
        Friends,
        Work,
        VIP
    }

    public enum GuestSide
    {
        Bride,
        Groom,
        Shared
    }

    public enum MealPreference
    {
        Standard,
        Vegetarian,
        Vegan,
        GlutenFree
    }
}