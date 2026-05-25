using SmartWorkingTracker.Core.Enums;
using SmartWorkingTracker.Core.Models;
using SmartWorkingTracker.Data.Database;
using SQLite;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;

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

        public Task<List<WorkSession>> GetSessionsByYear(int year)
        {

            var start = new DateTime(year, 1, 1);
            var end = start.AddYears(1);

            return _database.Table<WorkSession>()
                .Where(s => s.StartDate >= start && s.StartDate < end)
                .ToListAsync();

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

        #region TESTDATA

        public async Task PopulateDatabase()
        {
            try
            {
                if ((await GetAllSessions()).Count == 0)
                {
                    var sessions = new List<WorkSession>();


                    string[] righe = Constants.Csv.Split(
                        new[] { "\r\n", "\r", "\n" },
                        StringSplitOptions.None
                    );

                    for (int k = 1; k < righe.Length; k++)
                    {
                        string[] values = righe[k].Split(';');

                        string[] splittedStart = values[6].Split(" ");
                        DateTime startDate = DateTime.Parse(splittedStart[0], new CultureInfo("it-IT"));
                        startDate = startDate.AddHours(int.Parse(splittedStart[1].Split(":")[0].Replace("0", "")));
                        startDate = startDate.AddMinutes(int.Parse(splittedStart[1].Split(":")[1]));

                        string[] splittedFinish = values[7].Split(" ");
                        DateTime finishDate = DateTime.Parse(splittedFinish[0], new CultureInfo("it-IT"));
                        finishDate = finishDate.AddHours(int.Parse(splittedFinish[1].Split(":")[0].Replace("0", "")));
                        finishDate = finishDate.AddMinutes(int.Parse(splittedFinish[1].Split(":")[1]));

                        sessions.Add(new WorkSession
                        {
                            Serial = Guid.NewGuid().ToString(),
                            StartDate = startDate,
                            EndDate = finishDate,
                            Notes = $"{values[3]} - {values[4]}\n{values[8]} - {values[9]}\n{values[5]}",
                            Type = string.IsNullOrEmpty(values[2]) ? SessionType.NonPresente : values[2] == "MTR" ? SessionType.Presenza : SessionType.SmartWorking
                        });


                    }


                    foreach (var session in sessions)
                    {
                        await SaveSession(session);
                    }
                }


                if ((await GetContracts()).Count == 0)
                {
                    for (int year = DateTime.Now.Year - 5; year <= DateTime.Now.Year; year++)
                    {
                        await SaveContract(new Contract
                        {
                            Year = year,
                            WeeklyLimitHours = 24,
                            MonthlyLimitHours = 96, // 24 hours/week * 4 weeks
                            YearlyLimitHours = 960
                        });
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}
