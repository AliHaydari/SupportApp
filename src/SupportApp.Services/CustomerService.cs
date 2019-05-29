using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNTPersianUtils.Core;
using Microsoft.EntityFrameworkCore;
using SupportApp.Common.Extensions;
using SupportApp.Common.GuardToolkit;
using SupportApp.DataLayer.Context;
using SupportApp.Entities;
using SupportApp.Services.Contracts;
using SupportApp.ViewModels;

namespace SupportApp.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<Customer> _customers;

        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork.CheckArgumentIsNull(nameof(_unitOfWork));

            _customers = _unitOfWork.Set<Customer>();
            _customers.CheckArgumentIsNull(nameof(_customers));
        }

        public async Task<List<CustomerViewModel>> GetAllAsync()
        {
            return await _customers
                .Include(p => p.SoftwareVersion)
                .Include(p => p.LockVersion)
                .Select(p => new CustomerViewModel()
                {
                    Id = p.Id,
                    Number = p.Number,
                    Name = p.Name,
                    Family = p.Family,
                    SoftwareVersionId = p.SoftwareVersionId,
                    SoftwareVersionName = p.SoftwareVersion.Name,
                    SoftwareVersionReleaseNote = p.SoftwareVersion.ReleaseNote,
                    LockNumber = p.LockNumber,
                    LockVersionId = p.LockVersionId,
                    LockVersionName = p.LockVersion.Name,
                    AccountCount = p.AccountCount,
                    CompanyCount = p.CompanyCount,
                    Address = p.Address,
                    Tell = p.Tell,
                    DateOfSupportEndYear = p.SupportEndDate.ToPersianYearMonthDay(DateTimeOffsetPart.DateTime).Year,
                    DateOfSupportEndMonth = p.SupportEndDate.ToPersianYearMonthDay(DateTimeOffsetPart.DateTime).Month,
                    DateOfSupportEndDay = p.SupportEndDate.ToPersianYearMonthDay(DateTimeOffsetPart.DateTime).Day,
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<CustomerViewModel> GetByIdAsync(int id)
        {
            var entity = await _customers
                .Include(p => p.SoftwareVersion)
                .Include(p => p.LockVersion)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (entity != null)
            {
                var pDate = entity.SupportEndDate.ToPersianYearMonthDay(DateTimeOffsetPart.DateTime);
                return new CustomerViewModel()
                {
                    Id = entity.Id,
                    Number = entity.Number,
                    Name = entity.Name,
                    Family = entity.Family,
                    SoftwareVersionId = entity.SoftwareVersionId,
                    SoftwareVersionName = entity.SoftwareVersion.Name,
                    SoftwareVersionReleaseNote = entity.SoftwareVersion.ReleaseNote,
                    LockNumber = entity.LockNumber,
                    LockVersionId = entity.LockVersionId,
                    LockVersionName = entity.LockVersion.Name,
                    AccountCount = entity.AccountCount,
                    CompanyCount = entity.CompanyCount,
                    Address = entity.Address,
                    Tell = entity.Tell,
                    DateOfSupportEndYear = pDate.Year,
                    DateOfSupportEndMonth = pDate.Month,
                    DateOfSupportEndDay = pDate.Day,
                };
            }

            return null;
        }

        public async Task<bool> InsertAsync(CustomerViewModel viewModel)
        {
            var date = DateTimeExtensions.GetDateTimeOffset(
                viewModel.DateOfSupportEndYear,
                viewModel.DateOfSupportEndMonth,
                viewModel.DateOfSupportEndDay);

            var entity = new Customer()
            {
                Id = viewModel.Id,
                Number = viewModel.Number,
                Name = viewModel.Name,
                Family = viewModel.Family,
                SoftwareVersionId = viewModel.SoftwareVersionId,
                LockNumber = viewModel.LockNumber,
                LockVersionId = viewModel.LockVersionId,
                AccountCount = viewModel.AccountCount,
                CompanyCount = viewModel.CompanyCount,
                Address = viewModel.Address,
                Tell = viewModel.Tell,
                SupportEndDate = date,
            };

            await _customers.AddAsync(entity);
            var result = await _unitOfWork.SaveChangesAsync();
            return result != 0;
        }

        public async Task<bool> UpdateAsync(CustomerViewModel viewModel)
        {
            var entity = await _customers.FindAsync(viewModel.Id);

            if (entity != null)
            {
                var date = DateTimeExtensions.GetDateTimeOffset(
                    viewModel.DateOfSupportEndYear,
                    viewModel.DateOfSupportEndMonth,
                    viewModel.DateOfSupportEndDay);

                entity.Number = viewModel.Number;
                entity.Name = viewModel.Name;
                entity.Family = viewModel.Family;
                entity.SoftwareVersionId = viewModel.SoftwareVersionId;
                entity.LockNumber = viewModel.LockNumber;
                entity.LockVersionId = viewModel.LockVersionId;
                entity.AccountCount = viewModel.AccountCount;
                entity.CompanyCount = viewModel.CompanyCount;
                entity.Address = viewModel.Address;
                entity.Tell = viewModel.Tell;
                entity.SupportEndDate = date;

                var result = await _unitOfWork.SaveChangesAsync();
                return result != 0;
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _customers.FindAsync(id);

            if (entity != null)
            {
                _customers.Remove(entity);
                var result = await _unitOfWork.SaveChangesAsync();
                return result != 0;
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> CheckExistAsync(int id)
        {
            return await _customers.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> CheckExistNumberAsync(int? id, string number)
        {
            return id == null
                ? await _customers.AnyAsync(p => p.Number == number)
                : await _customers.AnyAsync(p => p.Id != id && p.Number == number);
        }

        public async Task<bool> CheckExistRelationAsync(int id)
        {
            //var result = await _customers
            //    .Include(p => p.Customers)
            //    .Where(p => p.Id == id)
            //    .AnyAsync(p => p.Customers.Any());

            return await Task.FromResult(false);
        }

    }
}
