using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QassimLabBooking.Models
{
    public class Lab
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<Device> Devices { get; set; } = new List<Device>();
    }
}