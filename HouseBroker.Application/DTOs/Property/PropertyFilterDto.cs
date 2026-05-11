using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBroker.Application.DTOs.Property
{
    public class PropertyFilterDto
    {
        public string? Title { get; set; }

        public string? PropertyType { get; set; }

        public string? Location { get; set; }

        public string? Features { get; set; }


        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }
    }
}
