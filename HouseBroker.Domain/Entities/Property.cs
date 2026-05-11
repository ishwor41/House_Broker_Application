
using HouseBroker.Domain.Enums;

namespace HouseBroker.Domain.Entities
{
    public class Property
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string PropertyType { get; set; }
        public string Location { get; set; }
        public decimal Price { get; set; }
        public string Features { get; set; }

        public decimal? CommissionAmount { get; set; }

        public string PropertyImageUrl { get; set; }

        public RecordStatus RecordStatus { get; set; } = RecordStatus.Active;

        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
