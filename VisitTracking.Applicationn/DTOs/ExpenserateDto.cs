using System;

namespace VisitTracking.Application.DTOs
{
    public  class ExpenserateDto
    {
        public int Id { get; set; } 
        public int VehicleTypeId { get; set; }
        public decimal RatePerKm { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public bool IsActive { get; set; }

    }
}
