using System.ComponentModel.DataAnnotations;

namespace RouletteBetting.Models
{
    public class User
    {
        [Key]
        public string? Username { get; set; }

        [Required]
        public decimal Balance { get; set; }
    }
}

