using AVS.Models.AdvertisementModels;
using System.ComponentModel.DataAnnotations;

namespace AVS.Models.AddressModels
{
    public class Address
    {
        public Guid ID { get; set; }

        [Required(ErrorMessage = "Обязательное поле!")]
        [Range(1, 500, ErrorMessage = "От 1 до 500")]
        public int HouseNumber { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Range(1, 100, ErrorMessage = "От 1 до 100")]
        public int Floor { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Range(1, 100, ErrorMessage = "От 1 до 100")]
        public int Entrance { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Range(1, 1000, ErrorMessage = "От 1 до 1000")]
        public int FlatNumber { get; set; }

        public Street? Street { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        public Guid? StreetID { get; set; }

        public List<Advertisement> Advertisements { get; set; } = [];
    }
}
