using System.ComponentModel.DataAnnotations;

namespace Club.Models
{
    public class CommitteeMember
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Department { get; set; } = string.Empty; // e.g., CSE, EEE, ME

        [Required]
        public string Role { get; set; } = string.Empty; // e.g., President, Secretary, Member
    }
}