using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayAPI.Models
{
    [Table("Organiser", Schema = "dbo")]
    public class Organiser
    {
        [Key]
        [ForeignKey(nameof(User))]
        public int OrganiserID { get; set; }

        [Required]
        [StringLength(100)]
        public string OrganisationName { get; set; } = string.Empty;

        public User? User { get; set; }
        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}