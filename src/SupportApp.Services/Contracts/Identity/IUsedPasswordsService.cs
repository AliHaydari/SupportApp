﻿using System;
using System.Threading.Tasks;
using SupportApp.Entities.Identity;

namespace SupportApp.Services.Contracts.Identity
{
    public interface IUsedPasswordsService
    {
        Task<bool> IsPreviouslyUsedPasswordAsync(User user, string newPassword);
        Task AddToUsedPasswordsListAsync(User user);
        Task<bool> IsLastUserPasswordTooOldAsync(int userId);
        Task<DateTimeOffset?> GetLastUserPasswordChangeDateAsync(int userId);
    }
}