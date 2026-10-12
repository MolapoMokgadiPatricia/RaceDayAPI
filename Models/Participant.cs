using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayAPI.Models
{
    [Table("Participant", Schema = "dbo")]
    public class Participant
    {
        [Key]
        public int ParticipantID { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(100)]
        public string? EmergencyContactName { get; set; }

        [StringLength(20)]
        public string? EmergencyContactPhone { get; set; }

        [StringLength(255)]
        public string? ProfilePictureUrl { get; set; }
    }
}