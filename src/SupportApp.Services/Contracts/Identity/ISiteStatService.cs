using System.Collections.Generic;
using System.Threading.Tasks;
using SupportApp.Entities.Identity;
using System.Security.Claims;
using SupportApp.ViewModels.Identity;

namespace SupportApp.Services.Contracts.Identity
{
    public interface ISiteStatService
    {
        Task<List<User>> GetOnlineUsersListAsync(int numbersToTake, int minutesToTake);

        Task<List<User>> GetTodayBirthdayListAsync();

        Task UpdateUserLastVisitDateTimeAsync(ClaimsPrincipal claimsPrincipal);

        Task<AgeStatViewModel> GetUsersAverageAge();
    }
}