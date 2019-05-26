using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNTPersianUtils.Core;
using Microsoft.EntityFrameworkCore;
using SupportApp.DataLayer.Context;
using SupportApp.Entities;
using SupportApp.Services.Contracts;
using SupportApp.ViewModels;

namespace SupportApp.Services
{
    public class SoftwareVersionService : ISoftwareVersionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<SoftwareVersion> _softwareVersions;

        public SoftwareVersionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _softwareVersions = _unitOfWork.Set<SoftwareVersion>();
        }

        public async Task<List<SoftwareVersionViewModel>> GetAllAsync()
        {
            return await _softwareVersions
                .Select(p => new SoftwareVersionViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    ReleaseNote = p.ReleaseNote,
                    Description = p.Description,
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<SoftwareVersionViewModel> GetByIdAsync(int id)
        {
            var entity = await _softwareVersions.FindAsync(id);

            if (entity != null)
            {
                return new SoftwareVersionViewModel()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    ReleaseNote = entity.ReleaseNote,
                    Description = entity.Description,
                };
            }

            return null;
        }

        public async Task<bool> InsertAsync(SoftwareVersionViewModel viewModel)
        {
            var entity = new SoftwareVersion()
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                ReleaseNote = viewModel.ReleaseNote,
                Description = viewModel.Description,
            };

            await _softwareVersions.AddAsync(entity);
            var result = await _unitOfWork.SaveChangesAsync();
            return result != 0;
        }

        public async Task<bool> UpdateAsync(SoftwareVersionViewModel viewModel)
        {
            var entity = await _softwareVersions.FindAsync(viewModel.Id);

            if (entity != null)
            {
                entity.Name = viewModel.Name;
                entity.ReleaseNote = viewModel.ReleaseNote;
                entity.Description = viewModel.Description;

                var result = await _unitOfWork.SaveChangesAsync();
                return result != 0;
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _softwareVersions.FindAsync(id);

            if (entity != null)
            {
                _softwareVersions.Remove(entity);
                var result = await _unitOfWork.SaveChangesAsync();
                return result != 0;
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> CheckExistAsync(int id)
        {
            return await _softwareVersions.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> CheckExistNameAsync(int? id, string name)
        {
            return id == null
                ? await _softwareVersions.AnyAsync(p => p.Name == name)
                : await _softwareVersions.AnyAsync(p => p.Id != id && p.Name == name);
        }

        public async Task<bool> CheckExistRelationAsync(int id)
        {
            var result = await _softwareVersions
                .Include(p => p.Customers)
                .Where(p => p.Id == id)
                .AnyAsync(p => p.Customers.Any());

            return await Task.FromResult(result);
        }

    }
}
