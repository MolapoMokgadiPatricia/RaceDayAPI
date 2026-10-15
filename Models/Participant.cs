using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayAPI.Models
{
    [Table("Participant", Schema = "dbo")]
    public class Participant
    {
        [Key]
        [ForeignKey(nameof(User))]
        public int ParticipantID { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(100)]
        public string? EmergencyContactName { get; set; }

        [StringLength(20)]
        public string? EmergencyContactPhone { get; set; }

        [StringLength(500)]
        public string? ProfilePictureUrl { get; set; }

        public User? User { get; set; }
        public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
    }
}