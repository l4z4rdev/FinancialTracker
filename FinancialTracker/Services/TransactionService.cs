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
            await _database.CreateTableAsync<DailyLimit>();

            // Osiguraj da postoji bar jedan zapis za dnevni limit
            var limits = await _database.Table<DailyLimit>().ToListAsync();
            if (limits.Count == 0)
            {
                await _database.InsertAsync(new DailyLimit { Amount = 5000, IsEnabled = false });
            }
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

        // Novi metodi za dnevni limit
        public async Task<DailyLimit> GetDailyLimit()
        {
            await Init();
            var limits = await _database.Table<DailyLimit>().ToListAsync();
            return limits.FirstOrDefault() ?? new DailyLimit { Amount = 5000, IsEnabled = false };
        }

        public async Task SaveDailyLimit(DailyLimit limit)
        {
            await Init();
            var existingLimit = await GetDailyLimit();

            if (existingLimit.Id == 0)
            {
                await _database.InsertAsync(limit);
            }
            else
            {
                limit.Id = existingLimit.Id;
                await _database.UpdateAsync(limit);
            }
        }

        // Provera da li će transakcija prekoračiti dnevni limit
        public async Task<(bool IsOverLimit, decimal DailyTotal, decimal Limit)> CheckDailyLimitExceeded(Transaction newTransaction)
        {
            if (newTransaction.Type != TransactionType.Expense)
                return (false, 0, 0);

            var limit = await GetDailyLimit();
            if (!limit.IsEnabled)
                return (false, 0, 0);

            // Uzmi period od 24h (trenutni dan)
            var twentyFourHoursAgo = DateTime.Now.AddHours(-24);

            var recentTransactions = await _database.Table<Transaction>()
                .Where(t => t.Type == TransactionType.Expense && t.Date >= twentyFourHoursAgo)
                .ToListAsync();

            decimal totalSpentToday = recentTransactions.Sum(t => t.Amount);

            // Dodaj i novu transakciju
            decimal potentialTotal = totalSpentToday + newTransaction.Amount;

            return (potentialTotal > limit.Amount, potentialTotal, limit.Amount);
        }
    }
}