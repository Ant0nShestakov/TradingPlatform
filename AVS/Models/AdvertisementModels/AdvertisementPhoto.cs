using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AVS.Models.AdvertisementModels
{
    public class AdvertisementPhoto
    {
        public Guid ID { get; set; }

        public string? FileName { get; set; } 

        [Required(ErrorMessage = "Обязательное поле!")]
        public string Path { get; set; } = string.Empty;
        public Advertisement? Advertisement { get; set; }
        public Guid AdvertisementsId { get; set; }

        public AdvertisementPhoto(string fileName, string path) 
        {
            this.FileName = fileName;
            this.Path = path;
        }
    }
}
