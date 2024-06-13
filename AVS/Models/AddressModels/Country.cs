using System.ComponentModel.DataAnnotations;

namespace AVS.Models.AddressModels
{
    public class Country
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Обязательное поле!")]
        [StringLength(64)]
        public string Name { get; set; } = string.Empty;
        public List<Region> Regions { get; set; } = [];
    }
}
