using System.ComponentModel.DataAnnotations;

namespace AVS.Models.AdvertisementModels
{
    public class AdvertisementState
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Advertisement> Advertisements { get; set; } = [];
    }
}
