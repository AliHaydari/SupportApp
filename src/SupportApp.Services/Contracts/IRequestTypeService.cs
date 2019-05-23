using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SupportApp.ViewModels;

namespace SupportApp.Services.Contracts
{
    public interface IRequestTypeService
    {
        Task<List<RequestTypeViewModel>> GetAllAsync();
        Task<RequestTypeViewModel> GetByIdAsync(int id);
        Task<bool> InsertAsync(RequestTypeViewModel viewModel);
        Task<bool> UpdateAsync(RequestTypeViewModel viewModel);
        Task<bool> DeleteAsync(int id);
        Task<bool> CheckExistAsync(int id);
        Task<bool> CheckExistNameAsync(int? id, string name);
    }
}
