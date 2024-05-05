using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AVS.Models.AddressModels
{
    public class Locality
    {
        public Guid ID { get; set; }

        [Required(ErrorMessage = "Обязательное поле!")]
        [StringLength(64)]
        public string Name { get; set; } = string.Empty;
        public Region? Region { get; set; }
        public Guid RegionID { get; set; }
        public List<Street>? Streets { get; set; }
    }
}
