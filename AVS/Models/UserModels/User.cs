using AVS.Models.AdvertisementModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AVS.Models.UserModels
{
    public class User
    {
        public Guid Id { get; set; }

        [StringLength(64)]
        [Required(ErrorMessage = "Обязательное поле!")]
        public string Name { get; set; } = string.Empty;


        [StringLength(64)]
        [Required(ErrorMessage = "Обязательное поле!")]
        public string SecondName { get; set; } = string.Empty;

        [StringLength(64)]
        public string ThirdName { get; set; } = string.Empty;

        [StringLength(64)]
        [Required(ErrorMessage = "Обязательное поле!")]
        public string Email { get; set; } = string.Empty;

        [StringLength(64)]
        [Required(ErrorMessage = "Обязательное поле!")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Не верный формат номера телефона!")]
        public string NumberPhone { get; set; } = string.Empty;

        [StringLength(64)]
        [Required(ErrorMessage = "Обязательное поле!")]
        public string Password { get; set; } = string.Empty;

        [NotMapped]
        [StringLength(64)]
        [Compare(nameof(Password), ErrorMessage = "Пароли должны совпадать!")]
        public string? ConfirmPassword { get; set; }

        public List<Advertisement>? Advertisements { get; set; }

        public List<Role>? Roles { get; set; }

        public List<Message>? Messages { get; set; }
    }
}
