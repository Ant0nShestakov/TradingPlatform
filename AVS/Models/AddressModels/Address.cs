using AVS.Models.AdvertisementModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AVS.Models.AddressModels
{
    public class Address
    {
        public Guid ID { get; set; }

        [Required(ErrorMessage = "Обязательное поле!")]
        public int HouseNumber { get; set; }

        [Range(1, 100, ErrorMessage = "От 1 до 100")]
        public int Floor { get; set; }

        [Range(1, 100, ErrorMessage = "От 1 до 100")]
        public int Entrance { get; set; }

        [Range(1, 1000, ErrorMessage = "От 1 до 1000")]
        public int FlatNumber { get; set; }

        public Street? Street { get; set; }

        public Guid StreetID { get; set; }

        public List<Advertisement>? Advertisements { get; set; }
    }
}
