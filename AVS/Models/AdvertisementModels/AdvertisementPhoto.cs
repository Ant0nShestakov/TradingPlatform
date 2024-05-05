using System.ComponentModel.DataAnnotations;

namespace AVS.Models.AdvertisementModels
{
    public class AdvertisementPhoto
    {
        public Guid ID { get; set; }
        [Required(ErrorMessage = "Обязательное поле!")]
        public string Path { get; set; } = string.Empty;
        public Advertisement? Advertisement { get; set; }
        public Guid AdvertisementsId { get; set; }
    }
}
