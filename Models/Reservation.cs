using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QassimLabBooking.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الطالب مطلوب")]
        public string StudentName { get; set; } = string.Empty;

        [Required(ErrorMessage = "يجب اختيار المعمل")]
        public int LabId { get; set; }

        [ForeignKey("LabId")]
        public virtual Lab? Lab { get; set; }

        [Required(ErrorMessage = "يجب اختيار الجهاز")]
        public int DeviceId { get; set; }

        [ForeignKey("DeviceId")]
        public virtual Device? Device { get; set; }

        [Required(ErrorMessage = "تاريخ الحجز مطلوب")]
        public string BookingDate { get; set; } = string.Empty;

        [Required(ErrorMessage = "وقت البداية مطلوب")]
        public string StartTime { get; set; } = string.Empty;

        [Required(ErrorMessage = "وقت النهاية مطلوب")]
        public string EndTime { get; set; } = string.Empty;

        public string Status { get; set; } = "مقبول";
    }
}