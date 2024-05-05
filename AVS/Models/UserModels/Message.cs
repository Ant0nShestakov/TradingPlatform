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
        public List<User> Users { get; set; } = [];
    }
}
