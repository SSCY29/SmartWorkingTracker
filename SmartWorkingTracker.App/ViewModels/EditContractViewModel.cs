using SmartWorkingTracker.Core.Models;
using SmartWorkingTracker.Data.Services;

namespace SmartWorkingTracker.App.ViewModels
{
    public class EditContractViewModel : BaseViewModel
    {
        private readonly DatabaseService _service;

        public int Year { get; set; }
        public double WeeklyLimitHours { get; set; }
        public double MonthlyLimitHours { get; set; }
        public double YearlyLimitHours { get; set; }
        public int? Id { get => _Id; private set => _Id = value; }

        private int? _Id;


        private Contract _existing;

        public EditContractViewModel(DatabaseService service)
        {
            _service = service;
        }

        public void SetId(int id)
        {
            Id = id;
        }

        public async Task Load()
        {
            IsLoading = true;

            try
            {
                _existing = null;
                if (_Id.HasValue)
                {
                    var contracts = await _service.GetContracts();
                    var contract = contracts.FirstOrDefault(c => c.Id == _Id.Value);
                    _existing = contract;
                }

                if (_existing == null)
                {
                    _existing = new Contract()
                    {
                        Year = DateTime.Now.Year,
                        WeeklyLimitHours = 40,
                        MonthlyLimitHours = 160,
                        YearlyLimitHours = 1920
                    };
                }

                Year = _existing.Year;
                WeeklyLimitHours = _existing.WeeklyLimitHours;
                MonthlyLimitHours = _existing.MonthlyLimitHours;
                YearlyLimitHours = _existing.YearlyLimitHours;
            }
            catch (Exception)
            {

            }
            finally
            {
                IsLoading = false;
            }

        }


        public async Task<bool> Save()
        {
            IsLoading = true;
            bool result = true;
            try
            {

                var contract = _existing ?? new Contract();

                contract.Year = Year;
                contract.WeeklyLimitHours = WeeklyLimitHours;
                contract.MonthlyLimitHours = MonthlyLimitHours;
                contract.YearlyLimitHours = YearlyLimitHours;

                // ✅ VALIDAZIONE ANNO UNICO
                var existing = await _service.GetContracts();

                if (existing.Any(c => c.Year == Year && c.Id != contract.Id))
                    result = false;
                else
                {
                    await _service.SaveContract(contract);
                }             

            }
            catch (Exception)
            {
                result = false;
            }
            finally
            {
                await Task.Delay(500);
                IsLoading = false;
            }


            return result;
        }
    }
}
