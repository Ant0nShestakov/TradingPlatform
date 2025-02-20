﻿using System.ComponentModel.DataAnnotations;

namespace AVS.Models.UserModels
{
    public class Permission
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Обязательное поле!")]
        [StringLength(64)]
        public string Name { get; set; } = string.Empty;

        public List<Role> Roles { get; set; } = [];
    }
}
