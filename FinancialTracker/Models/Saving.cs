using System.ComponentModel.DataAnnotations;
using SQLite;

namespace FinancialTracker.Models
{
    public class Saving
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Required(ErrorMessage = "Unesite naziv")]
        [System.ComponentModel.DataAnnotations.MaxLength(50, ErrorMessage = "Naziv štednje ne može biti duže od 50 karaktera")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Unesite iznos")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Iznos mora biti veći od 0")]
        public decimal TargetAmount { get; set; }

        public decimal CurrentAmount { get; set; } = 0;

        public bool IsCompleted => CurrentAmount >= TargetAmount;
    }
}
