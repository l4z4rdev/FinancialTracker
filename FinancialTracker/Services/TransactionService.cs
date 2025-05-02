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
            await _database.CreateTableAsync<Category>();
            await _database.CreateTableAsync<Saving>();
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
        public async Task UpdateTransaction(Transaction transaction)
        {
            await Init();
            await _database.UpdateAsync(transaction);
        }

        public async Task DeleteTransaction(int id)
        {
            await Init();
            await _database.DeleteAsync<Transaction>(id);
        }

        public async Task DeleteAllTransactions()
        {
            await Init();
            var transactions = await GetTransactions();
            foreach (var transaction in transactions)
            {
                await _database.DeleteAsync(transaction);
            }
        }


        public async Task<List<Category>> GetCategories()
        {
            await Init();
            return await _database.Table<Category>().OrderBy(c => c.Name).ToListAsync();
        }

        public async Task AddCategory(Category category)
        {
            await Init();
            await _database.InsertAsync(category);
        }

        public async Task UpdateCategory(Category category)
        {
            await Init();
            await _database.UpdateAsync(category);
        }

        public async Task DeleteCategory(int id)
        {
            await Init();
            await _database.DeleteAsync<Category>(id);
        }

        public async Task DeleteAllCategories()
        {
            await Init();
            var categories = await GetCategories();
            foreach (var category in categories)
            {
                await _database.DeleteAsync(category);
            }
        }


        public async Task<List<Saving>> GetSavings()
        {
            await Init();
            return await _database.Table<Saving>().OrderByDescending(s => s.Id).ToListAsync();
        }

        public async Task AddSaving(Saving saving)
        {
            await Init();
            await _database.InsertAsync(saving);
        }

        public async Task UpdateSaving(Saving saving)
        {
            await Init();
            await _database.UpdateAsync(saving);
        }

        public async Task DeleteSaving(int id)
        {
            await Init();
            await _database.DeleteAsync<Saving>(id);
        }

        public async Task DeleteAllSavings()
        {
            await Init();
            var savings = await GetSavings();
            foreach (var saving in savings)
            {
                await _database.DeleteAsync(saving);
            }
        }
    }
}
