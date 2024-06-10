using AVS.Models.AdvertisementModels;
using System.ComponentModel.DataAnnotations;

namespace AVS.Models.UserModels
{
    public class Message
    {
        public Guid Id { get; set; }
        public DateTime? CreatedAt { get; set; }

        [Required]
        [StringLength(2000)]
        public string Content { get; set; } = string.Empty;
        public Guid ReceiverUserId { get; set; }
        public Guid SenderUserId { get; set; }
        public Guid AdvertisementId { get; set; }
        public Advertisement? Advertisement { get; set; }
        public User? ReceiverUser { get; set; }
        public User? SenderUser { get; set; }

    }
}
