using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SupportApp.Common.GuardToolkit;
using SupportApp.DataLayer.Context;
using SupportApp.Entities;
using SupportApp.Services.Contracts;
using SupportApp.ViewModels;

namespace SupportApp.Services
{
    public class RequestTypeService : IRequestTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<RequestType> _requestTypes;

        public RequestTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork.CheckArgumentIsNull(nameof(_unitOfWork));

            _requestTypes = _unitOfWork.Set<RequestType>();
            _requestTypes.CheckArgumentIsNull(nameof(_requestTypes));
        }

        public async Task<List<RequestTypeViewModel>> GetAllAsync()
        {
            return await _requestTypes
                .Select(p => new RequestTypeViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<RequestTypeViewModel> GetByIdAsync(int id)
        {
            var entity = await _requestTypes.FindAsync(id);

            if (entity != null)
            {
                return new RequestTypeViewModel()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,
                };
            }

            return null;
        }

        public async Task<bool> InsertAsync(RequestTypeViewModel viewModel)
        {
            var entity = new RequestType()
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Description = viewModel.Description,
            };

            await _requestTypes.AddAsync(entity);
            var result = await _unitOfWork.SaveChangesAsync();
            return result != 0;
        }

        public async Task<bool> UpdateAsync(RequestTypeViewModel viewModel)
        {
            var entity = await _requestTypes.FindAsync(viewModel.Id);

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
            var entity = await _requestTypes.FindAsync(id);

            if (entity != null)
            {
                _requestTypes.Remove(entity);
                var result = await _unitOfWork.SaveChangesAsync();
                return result != 0;
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> CheckExistAsync(int id)
        {
            return await _requestTypes.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> CheckExistNameAsync(int? id, string name)
        {
            return id == null
                ? await _requestTypes.AnyAsync(p => p.Name == name)
                : await _requestTypes.AnyAsync(p => p.Id != id && p.Name == name);
        }

    }
}
