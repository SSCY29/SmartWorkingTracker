using SmartWorkingTracker.Core.Models;
using SmartWorkingTracker.Data.Services;
using System.Collections.ObjectModel;

namespace SmartWorkingTracker.App.ViewModels
{
    public class ContractViewModel : BaseViewModel
    {
        private readonly DatabaseService _service;

        public ObservableCollection<Contract> Contracts { get; set; } = new();

        public ContractViewModel(DatabaseService service)
        {
            _service = service;
        }

        public async Task Load()
        {
            IsLoading = true;

            try
            {
                Contracts.Clear();

                var list = await _service.GetContracts();

                foreach (var c in list.OrderByDescending(c => c.Year))
                    Contracts.Add(c);
            }
            catch (Exception)
            {                
            }
            finally
            {
                IsLoading = false;
            }

        }

        public async Task Save(Contract contract)
        {
            IsLoading = true;

            try
            {
                await _service.SaveContract(contract);
                await Load();
            }
            catch (Exception)
            {
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task Delete(Contract contract)
        {
            IsLoading = true;

            try
            {
                await _service.DeleteContract(contract);
                Contracts.Remove(contract);
            }
            catch (Exception)
            {
            }
            finally
            {
                IsLoading = false;
            }
        }
    }

}
