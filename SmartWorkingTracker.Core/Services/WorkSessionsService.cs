using SmartWorkingTracker.Data.Database;
using SmartWorkingTracker.Data.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

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
            return _database.Table<WorkSessionEntity>()
                            .Where(i => i.From >= date && i.To <= date)
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

        public Task InsertOrUpdateAsync(WorkSessionEntity entity)
        {
            return _database.InsertOrReplaceAsync(entity);
            
        }
    }
}
