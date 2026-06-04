using System.ComponentModel.DataAnnotations;

namespace Club.Models
{
    public class InventoryItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ItemName { get; set; } = string.Empty; // e.g., "Logitech Presenter"
        [Required]
        public string AssignedToDepartment { get; set; } = string.Empty; // e.g., "CSE"
        public string ServiceTrackerId { get; set; } = string.Empty; // To trace the lifetime ID
    }
}