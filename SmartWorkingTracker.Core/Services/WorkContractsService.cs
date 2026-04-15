using SmartWorkingTracker.Data.Database;
using SmartWorkingTracker.Data.Entities;
using SQLite;

namespace SmartWorkingTracker.Core.Services
{
    public class WorkContractsService
    {
        private readonly SQLiteAsyncConnection _database;
        public WorkContractsService(AppDatabase database)
        {
            _database = database.Connection;
        }

        public Task<List<WorkContractEntity>> GetContractsAsync()
        {
            return _database.Table<WorkContractEntity>().ToListAsync();
        }

        public Task<WorkContractEntity> GetContractByYearAsync(int year)
        {            
            return _database.Table<WorkContractEntity>()
                            .FirstOrDefaultAsync(i => i.Year == year);
                            
        }

        public Task InsertOrUpdateAsync(WorkContractEntity entity)
        {
            return _database.InsertOrReplaceAsync(entity);

        }
    }
}
