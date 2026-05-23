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
            Contracts.Clear();

            var list = await _service.GetContracts();

            foreach (var c in list.OrderBy(c => c.Year))
                Contracts.Add(c);
        }

        public async Task Save(Contract contract)
        {
            await _service.SaveContract(contract);
            await Load();
        }

        public async Task Delete(Contract contract)
        {
            await _service.DeleteContract(contract);
            Contracts.Remove(contract);
        }
    }

}
