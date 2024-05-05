using System.ComponentModel.DataAnnotations;

namespace AVS.Models.UserModels
{
    public class Role
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [StringLength(64)]
        public string Name { get; set; } = string.Empty;

        public List<Permission>? Permissions { get; set; }
        public List<User>? Users { get; set; }
    }
}
