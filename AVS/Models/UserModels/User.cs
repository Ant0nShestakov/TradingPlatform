using AVS.Models.AdvertisementModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AVS.Models.UserModels
{
    public class User
    {
        public Guid Id { get; set; }

        [StringLength(15)]
        public string Name { get; set; } = string.Empty;

        [StringLength(30)]
        public string SecondName { get; set; } = string.Empty;

        [StringLength(15)]
        public string ThirdName { get; set; } = string.Empty;

        [StringLength(64)]
        public string Email { get; set; } = string.Empty;

        [StringLength(64)]
        public string NumberPhone { get; set; } = string.Empty;

        [StringLength(64)]
        public string Password { get; set; } = string.Empty;

        [NotMapped]
        [StringLength(64)]
        public string ConfirmPassword { get; set; } = string.Empty;

        public List<Advertisement> Advertisements { get; set; } = [];

        public List<Role> Roles { get; set; } = [];

        public List<Message> Messages { get; set; } = [];
    }
}
