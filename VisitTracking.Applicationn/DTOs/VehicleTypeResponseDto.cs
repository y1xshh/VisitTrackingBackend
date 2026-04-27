using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitTracking.Application.DTOs
{
    public class VehicleTypeResponseDto
    {
        public int Id { get; set; }
        public string? VehicleName { get; set; }
        public decimal? DefaultRatePerKm { get; set; }
        public bool? IsActive { get; set; }

    }
}
