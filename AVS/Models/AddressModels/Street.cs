using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AVS.Models.AddressModels
{
    public class Street
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Обязательное поле!")]
        [StringLength(64)]
        public string Name { get; set; } = string.Empty;
        public Locality? Locality { get; set; }
        public Guid LocalityID { get; set; }
        public List<Address>? Addresses { get; set; }
    }
}
