using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SupportApp.ViewModels;

namespace SupportApp.Services.Contracts
{
    public interface ICustomerService
    {
        Task<List<CustomerViewModel>> GetAllAsync();
        Task<CustomerViewModel> GetByIdAsync(int id);
        Task<bool> InsertAsync(CustomerViewModel viewModel);
        Task<bool> UpdateAsync(CustomerViewModel viewModel);
        Task<bool> DeleteAsync(int id);
        Task<bool> CheckExistAsync(int id);
        Task<bool> CheckExistNumberAsync(int? id, string number);
        Task<bool> CheckExistRelationAsync(int id);
    }
}
