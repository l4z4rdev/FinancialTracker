using System.ComponentModel.DataAnnotations;
using SQLite;

namespace FinancialTracker.Models
{
    public class DailyLimit
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Limit mora biti veći od 0")]
        public decimal Amount { get; set; }

        public bool IsEnabled { get; set; } = true;
    }
}