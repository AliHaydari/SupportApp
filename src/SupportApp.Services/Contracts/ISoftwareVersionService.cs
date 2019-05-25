using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SupportApp.ViewModels;

namespace SupportApp.Services.Contracts
{
    public interface ISoftwareVersionService
    {
        Task<List<SoftwareVersionViewModel>> GetAllAsync();
        Task<SoftwareVersionViewModel> GetByIdAsync(int id);
        Task<bool> InsertAsync(SoftwareVersionViewModel viewModel);
        Task<bool> UpdateAsync(SoftwareVersionViewModel viewModel);
        Task<bool> DeleteAsync(int id);
        Task<bool> CheckExistAsync(int id);
        Task<bool> CheckExistNameAsync(int? id, string name);
        Task<bool> CheckExistRelationAsync(int id);
    }
}
