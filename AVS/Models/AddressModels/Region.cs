using System.ComponentModel.DataAnnotations;

namespace AVS.Models.AddressModels
{
    public class Region
    {
        public Guid ID { get; set; }

        [StringLength(64)]
        public string Name { get; set; } = string.Empty;
        public Country? Country { get; set; }

        [Required(ErrorMessage = "Обязательное поле!")]
        public Guid CountryID { get; set; }

        public List<Locality> Localitys { get; set; } = [];
    }
}
