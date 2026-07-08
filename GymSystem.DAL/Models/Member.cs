namespace GymSystem.DAL.Models
{
    public class Member : GymUser
    {
        // Inherited: Id, Name, Email, Phone, DateOfBirth, Gender, Address

        public string? Photo { get; set; }   // optional profile picture

        public DateTime JoinDate { get; set; }   // auto-set on insert

        // Navigation: 1-to-1 with HealthRecord
        public HealthRecord? HealthRecord { get; set; }

        // Navigation: M-M with Plan via Membership junction
        public ICollection<Membership> Memberships { get; set; } = new HashSet<Membership>();

        // Navigation: M-M with Session via Booking junction
        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
    }
}