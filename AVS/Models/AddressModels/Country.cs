using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AVS.Models.AddressModels
{
    [Table(nameof(Country))]
    public class Country
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Обязательное поле!")]
        [StringLength(64)]
        public string Name { get; set; } = string.Empty;
        public List<Region>? Regions { get; set; }
    }
}
