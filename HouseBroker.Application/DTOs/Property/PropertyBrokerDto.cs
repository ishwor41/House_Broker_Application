using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBroker.Application.DTOs.Property
{
    public class PropertyBrokerDto
    {
        public string Title { get; set; }

        public string Features { get; set; }

        public string Location { get; set; }
        public decimal Price { get; set; }
        public decimal? Commission { get; set; }

        public string ImagePath { get; set; }
    }
}
