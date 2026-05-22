using SmartWorkingTracker.Core.Models;
using SmartWorkingTracker.Data.Database;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartWorkingTracker.Data.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;
        public DatabaseService(AppDatabase database)
        {
            _database = database.Connection;
        }

        #region WORK SESSIONS
        public Task<List<WorkSession>> GetAllSessions()
        {
            return _database.Table<WorkSession>().ToListAsync();
        }

        public Task<List<WorkSession>> GetSessionsByMonth(int year, int month)
        {

            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1);

            return _database.Table<WorkSession>()
                .Where(s => s.StartDate >= start && s.StartDate < end)
                .ToListAsync();

        }
        public Task<List<WorkSession>> GetSessionsByDate(int year, int month, int day)
        {

            var start = new DateTime(year, month, day);
            var end = start.AddMonths(1);

            return _database.Table<WorkSession>()
                .Where(s => s.StartDate >= start && s.StartDate < end)
                .ToListAsync();

        }

        public Task<List<WorkSession>> GetSessionById(int id)
        {            
            return _database.Table<WorkSession>()
                .Where(s => s.Id == id)
                .ToListAsync();

        }

        public Task<int> SaveSession(WorkSession session)
        {
            if (session.Id == 0)
                return _database.InsertAsync(session);
            else
                return _database.UpdateAsync(session);
        }

        public Task<int> DeleteSession(WorkSession session)
        {
            return _database.DeleteAsync(session);
        }

        #endregion

        #region CONTRACT

        public Task<List<Contract>> GetContracts()
        {
            return _database.Table<Contract>().ToListAsync();
        }

        public Task<Contract> GetContractByYear(int year)
        {
            return _database.Table<Contract>()
                .FirstOrDefaultAsync(c => c.Year == year);
        }

        public Task<int> SaveContract(Contract contract)
        {
            if (contract.Id == 0)
                return _database.InsertAsync(contract);
            else
                return _database.UpdateAsync(contract);
        }

        public Task<int> DeleteContract(Contract contract)
        {
            return _database.DeleteAsync(contract);
        }

        #endregion

    }
}
