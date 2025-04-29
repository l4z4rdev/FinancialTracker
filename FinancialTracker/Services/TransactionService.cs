using SQLite;
using FinancialTracker.Models;

namespace FinancialTracker.Services
{
    public class TransactionService
    {
        private SQLiteAsyncConnection _database;

        public async Task Init()
        {
            if (_database != null)
                return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "transactions.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            await _database.CreateTableAsync<Transaction>();
        }

        public async Task<List<Transaction>> GetTransactions()
        {
            await Init();
            return await _database.Table<Transaction>().OrderByDescending(t => t.Date).ToListAsync();
        }

        public async Task AddTransaction(Transaction transaction)
        {
            await Init();
            await _database.InsertAsync(transaction);
        }

        public async Task DeleteTransaction(int id)
        {
            await Init();
            await _database.DeleteAsync<Transaction>(id);
        }
    }
}
