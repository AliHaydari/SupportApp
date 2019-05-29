using SupportApp.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.Text;
using SupportApp.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SupportApp.ViewModels;
using System.Linq;
using SupportApp.Services.Contracts;

namespace SupportApp.Services
{
    public class LockVersionService : ILockVersionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<LockVersion> _lockVersions;

        public LockVersionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _lockVersions = _unitOfWork.Set<LockVersion>();
        }

        public async Task<List<LockVersionViewModel>> GetAllAsync()
        {
            return await _lockVersions
                .Select(p => new LockVersionViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<LockVersionViewModel> GetByIdAsync(int id)
        {
            var entity = await _lockVersions.FindAsync(id);

            if (entity != null)
            {
                return new LockVersionViewModel()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,
                };
            }

            return null;
        }

        public async Task<bool> InsertAsync(LockVersionViewModel viewModel)
        {
            var entity = new LockVersion()
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Description = viewModel.Description,
            };

            await _lockVersions.AddAsync(entity);
            var result = await _unitOfWork.SaveChangesAsync();
            return result != 0;
        }

        public async Task<bool> UpdateAsync(LockVersionViewModel viewModel)
        {
            var entity = await _lockVersions.FindAsync(viewModel.Id);

            if (entity != null)
            {
                entity.Name = viewModel.Name;
                entity.Description = viewModel.Description;

                var result = await _unitOfWork.SaveChangesAsync();
                return result != 0;
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _lockVersions.FindAsync(id);

            if (entity != null)
            {
                _lockVersions.Remove(entity);
                var result = await _unitOfWork.SaveChangesAsync();
                return result != 0;
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> CheckExistAsync(int id)
        {
            return await _lockVersions.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> CheckExistNameAsync(int? id, string name)
        {
            return id == null
                ? await _lockVersions.AnyAsync(p => p.Name == name)
                : await _lockVersions.AnyAsync(p => p.Id != id && p.Name == name);
        }

        public async Task<bool> CheckExistRelationAsync(int id)
        {
            var result = await _lockVersions
                .Include(p => p.Customers)
                .Where(p => p.Id == id)
                .AnyAsync(p => p.Customers.Any());

            return await Task.FromResult(result);
        }
    }
}
