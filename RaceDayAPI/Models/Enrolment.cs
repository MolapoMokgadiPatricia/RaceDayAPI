using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayAPI.Models
{
    [Table("Enrolment", Schema = "dbo")]
    public class Enrolment
    {
        [Key]
        public int EnrolmentID { get; set; }

        [Required]
        public int ParticipantID { get; set; }

        [Required]
        public int EventID { get; set; }

        [Required]
        public int CategoryID { get; set; }

        public DateTime EnrolmentDate { get; set; } = DateTime.UtcNow;

        [StringLength(20)]
        public string? Status { get; set; } = "Pending";

        public Participant? Participant { get; set; }
        public Event? Event { get; set; }
        public Category? Category { get; set; }
        public Result? Result { get; set; }
    }
}