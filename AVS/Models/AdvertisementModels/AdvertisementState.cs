using System.ComponentModel.DataAnnotations;

namespace AVS.Models.AdvertisementModels
{
    public class AdvertisementState
    {
        public Guid ID { get; set; }
        [Required(ErrorMessage = "Обязательное поле!")]
        public string Name { get; set; } = string.Empty;
        public List<Advertisement> Advertisements { get; set; } = [];
    }
}
