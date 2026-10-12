using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayAPI.Models
{
    [Table("Organiser", Schema = "dbo")]
    public class Organiser
    {
        [Key]
        public int OrganiserID { get; set; }

        [Required]
        [StringLength(100)]
        public string OrganisationName { get; set; } = string.Empty;
    }
}