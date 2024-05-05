using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AVS.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(64)]
        [Required(ErrorMessage = "Обязательное поле!")]
        public string? Name { get; set; }


        [StringLength(64)]
        [Required(ErrorMessage = "Обязательное поле!")]
        public string? SecondName { get; set; }

        [StringLength(64)]
        public string? ThirdName { get; set; }


        [StringLength(64)]
        [Required(ErrorMessage = "Обязательное поле!")]
        public string? Email { get; set; }

        [StringLength(64)]
        [Required(ErrorMessage = "Обязательное поле!")]
        [DataType(DataType.PhoneNumber, ErrorMessage ="Не верный формат номера телефона!")]
        public string? NumberPhone { get; set; }

        public string? RefreshJWT { get; set; }

        [StringLength(64)]
        [Required(ErrorMessage = "Обязательное поле!")]
        public string? Password { get; set; }

        [NotMapped]
        [StringLength(64)]
        [Compare(nameof(Password), ErrorMessage = "Пароли должны совпадать!")]
        public string? ConfirmPassword { get; set; }
    }
}
