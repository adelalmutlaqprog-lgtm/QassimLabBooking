using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QassimLabBooking.Models
{
    public class Device
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public int LabId { get; set; }

        [ForeignKey("LabId")]
        public virtual Lab? Lab { get; set; }
    }
}