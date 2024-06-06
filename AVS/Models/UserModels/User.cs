using AVS.Models.AdvertisementModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace AVS.Models.UserModels
{
    public class User
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage ="Обязательное поле!")]
        [StringLength(15)]
        public string Name { get; set; } = string.Empty;

        [StringLength(30)]
        [Required(ErrorMessage = "Обязательное поле!")]
        public string SecondName { get; set; } = string.Empty;

        [StringLength(15)]
        [Required(ErrorMessage = "Обязательное поле!")]
        public string ThirdName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Обязательное поле!")]
        [StringLength(64)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Обязательное поле!")]
        [StringLength(64)]
        public string NumberPhone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Обязательное поле!")]
        [StringLength(64)]
        public string Password { get; set; } = string.Empty;

        [NotMapped]
        [Required(ErrorMessage = "Обязательное поле!")]
        [StringLength(64)]
        public string ConfirmPassword { get; set; } = string.Empty;

        public List<Advertisement> Advertisements { get; set; } = [];
        public List<Message> Messages { get; set; } = [];
        public List<Role> Roles { get; set; } = [];
    }
}
