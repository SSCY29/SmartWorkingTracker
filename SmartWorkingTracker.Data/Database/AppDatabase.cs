using SmartWorkingTracker.Data.Entities;
using SQLite;

namespace SmartWorkingTracker.Data.Database
{
    public class AppDatabase
    {
        private readonly SQLiteAsyncConnection _database;
        private readonly SemaphoreSlim _initLock = new(1, 1);
        private Task? _initializationTask;

        public SQLiteAsyncConnection Connection => _database;

        public AppDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(
                dbPath,
                SQLiteOpenFlags.ReadWrite |
                SQLiteOpenFlags.Create
            );
        }

        /// <summary>
        /// Garantisce che il DB sia inizializzato UNA SOLA VOLTA.
        /// Tutti gli await restano in attesa dello stesso Task.
        /// </summary>
        public Task InitializeAsync()
        {
            return _initializationTask ??= InitializeInternalAsync();
        }

        private async Task InitializeInternalAsync()
        {
            await _initLock.WaitAsync();
            try
            {
                //await _database.ExecuteAsync("PRAGMA foreign_keys = ON;");

                await _database.CreateTableAsync<WorkSessionEntity>();
                await _database.CreateTableAsync<WorkContractEntity>();
            }
            finally
            {
                _initLock.Release();
            }
        }
    }


}
