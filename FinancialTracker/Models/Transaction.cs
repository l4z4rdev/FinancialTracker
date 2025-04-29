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

        public decimal Amount { get; set; }

        public TransactionType Type { get; set; }

        public string Category { get; set; }

        public DateTime Date { get; set; }

        public string Note { get; set; }
    }
}
