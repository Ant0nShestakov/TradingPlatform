using AVS.Models.AddressModels;
using AVS.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace AVS.Models.AdvertisementModels
{
    public class Advertisement
    {
        public Guid ID { get; set; }

        [Required(ErrorMessage = "Обязательное поле!")]
        [StringLength(64)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Обязательное поле!")]
        [StringLength(2000)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Обязательное поле!")]
        [Range(0, float.MaxValue)]
        public float Price { get; set; }

        [Required(ErrorMessage = "Обязательное поле!")]
        public DateTime? CreatedDate { get; set; }

        [Range(0, int.MaxValue)]
        public int NumberOfViews { get; set; }

        public AdvertisementState? AdvertisementState { get; set; }

        [Required(ErrorMessage = "Обязательное поле!")]
        public Guid AdvertisementStateId {  get; set; }

        public Address? Address { get; set; }

        public Guid AddressId { get; set; }

        public Category? Category { get; set; }

        public Guid CategoryId { get; set; }

        public List<AdvertisementPhoto> Photos { get; set; } = [];

        public User? User { get; set; }
        public Guid UserId { get; set; }
    }
}
