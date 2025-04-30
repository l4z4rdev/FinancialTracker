using System.ComponentModel.DataAnnotations;
using SQLite;

namespace FinancialTracker.Models
{
    public enum TransactionType
    {
        Income,
        Expense
    }

    public class Transaction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [System.ComponentModel.DataAnnotations.MaxLength(100, ErrorMessage = "Naziv ne može biti duži od 100 karaktera")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Unesite iznos")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Iznos mora biti veći od 0")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Odaberite tip transakcije")]
        public TransactionType Type { get; set; }

        [Required(ErrorMessage = "Odaberite kategoriju")]
        public string Category { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [System.ComponentModel.DataAnnotations.MaxLength(500, ErrorMessage = "Napomena ne može biti duža od 500 karaktera")]
        public string Note { get; set; }
    }
}
