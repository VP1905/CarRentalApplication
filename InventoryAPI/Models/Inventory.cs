using System.ComponentModel.DataAnnotations;

namespace InventoryAPI.Models
{
    public class Inventory
    {
        public int VehicleId { get; set; }

        [Required]
        public string Make { get; set; }

        [Required]
        public string Model { get; set; }

        [Range(2000, 2100)]
        public int Year { get; set; }

        [Range(1, 1000)]
        public decimal DailyRate { get; set; }

        public bool IsAvailable { get; set; }
    }
}
