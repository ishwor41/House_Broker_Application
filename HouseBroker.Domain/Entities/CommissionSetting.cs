using HouseBroker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBroker.Domain.Entities
{
    public class CommissionSetting
    {
        public Guid Id { get; set; }

        public decimal MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public decimal Percentage { get; set; }

        public RecordStatus RecordStatus { get; set; } = RecordStatus.Active;
    }
}
