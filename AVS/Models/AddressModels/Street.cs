﻿using System.ComponentModel.DataAnnotations;

namespace AVS.Models.AddressModels
{
    public class Street
    {
        public Guid Id { get; set; }

        [StringLength(64)]
        public string Name { get; set; } = string.Empty;
        public Locality? Locality { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        public Guid LocalityID { get; set; }
        public List<Address> Addresses { get; set; } = [];
    }
}
