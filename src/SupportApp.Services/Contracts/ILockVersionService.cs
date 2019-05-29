using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SupportApp.ViewModels;

namespace SupportApp.Services.Contracts
{
    public interface ILockVersionService
    {
        Task<List<LockVersionViewModel>> GetAllAsync();
        Task<LockVersionViewModel> GetByIdAsync(int id);
        Task<bool> InsertAsync(LockVersionViewModel viewModel);
        Task<bool> UpdateAsync(LockVersionViewModel viewModel);
        Task<bool> DeleteAsync(int id);
        Task<bool> CheckExistAsync(int id);
        Task<bool> CheckExistNameAsync(int? id, string name);
        Task<bool> CheckExistRelationAsync(int id);
    }
}
