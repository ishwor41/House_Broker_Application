using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HouseBroker.Domain.Enums;

namespace HouseBroker.Application.DTOs.Property
{
    public class PropertyDto
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string PropertyType { get; set; }

        [Required]
        public string Location { get; set; }

        [Range(1, double.MaxValue)]
        public decimal Price { get; set; }

        public string Features { get; set; }

        [Required]
        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; }


        public IFormFile PropertyImage { get; set; }
    }
}
