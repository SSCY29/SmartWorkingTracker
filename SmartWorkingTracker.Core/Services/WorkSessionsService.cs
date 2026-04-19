using SmartWorkingTracker.Core.Models;
using SmartWorkingTracker.Data.Database;
using SmartWorkingTracker.Data.Entities;
using SQLite;
using static System.Net.Mime.MediaTypeNames;

namespace SmartWorkingTracker.Core.Services
{
    public class WorkSessionsService
    {
        private readonly SQLiteAsyncConnection _database;
        public WorkSessionsService(AppDatabase database)
        {
            _database = database.Connection;
        }

        public Task<List<WorkSessionEntity>> GetSessionsAsync()
        {
            return _database.Table<WorkSessionEntity>().ToListAsync();
        }

        public Task<List<WorkSessionEntity>> GetSessionByDateAsync(DateTime date)
        {
            var from = date;
            var to = from.AddDays(1);

            return _database.Table<WorkSessionEntity>()
                .Where(x => x.From >= from && x.From < to)
                .ToListAsync();
        }
        public Task<List<WorkSessionEntity>> GetSessionByMonthAsync(int year, int month)
        {
            var from = new DateTime(year, month, 1);
            var to = from.AddMonths(1);

            return _database.Table<WorkSessionEntity>()
                .Where(x => x.From >= from && x.From < to)
                .ToListAsync();

        }

        public Task<List<WorkSessionEntity>> GetSessionByYearAsync(int year)
        {
            return _database.Table<WorkSessionEntity>()
                            .Where(i => i.From.Year == year)
                            .ToListAsync();
        }

        public Task InsertAsync(WorkSessionEntity entity)
        {
            return _database.InsertAsync(entity);
            
        }

        public Task UpdateAsync(WorkSessionEntity session)
                => _database.UpdateAsync(session);

        public Task DeleteAsync(int sessionId)
                => _database.Table<WorkSessionEntity>().DeleteAsync(x => x.Id == sessionId);


        public Task<WorkSessionEntity> GetByIdAsync(int sessionId)
                => _database.Table<WorkSessionEntity>().FirstOrDefaultAsync(x => x.Id == sessionId);


    }
}
