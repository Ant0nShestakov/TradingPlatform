using System.ComponentModel.DataAnnotations;

namespace AVS.Models.AddressModels
{
    public class Region
    {
        public Guid ID { get; set; }

        [Required(ErrorMessage = "Обязательное поле!")]
        [StringLength(64)]
        public string Name { get; set; } = string.Empty;
        public Country? Country { get; set; }
        public Guid CountryID { get; set; }
        public List<Locality> Localitys { get; set; } = [];
    }
}
