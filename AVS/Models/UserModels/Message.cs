using System.ComponentModel.DataAnnotations;

namespace AVS.Models.UserModels
{
    public class Message
    {
        public Guid Id { get; set; }
        public User? UserSender { get; set; }
        public Guid UserSenderId { get; set; }
        public User? UserReciver { get; set; }
        public Guid UserReciverId { get; set; }
        public DateTime? CreatedAt { get; set; }

        [Required]
        [StringLength(2000)]
        public string Content { get; set; } = string.Empty;
    }
}
