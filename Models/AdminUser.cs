using System.ComponentModel.DataAnnotations;

namespace QassimLabBooking.Models
{
    public class AdminUser
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } // اسم المستخدم للأدمن

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } // كلمة المرور
    }
}