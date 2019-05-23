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
        Task InsertAsync(RequestTypeViewModel viewModel);
        Task UpdateAsync(int id, RequestTypeViewModel viewModel);
        Task DeleteAsync(int id);
        Task<bool> CheckExistAsync(int id);
        Task<bool> CheckExistNameAsync(int? id, string name);
    }
}
