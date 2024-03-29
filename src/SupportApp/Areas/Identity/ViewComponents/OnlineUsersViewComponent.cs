﻿using System.Threading.Tasks;
using SupportApp.Services.Contracts.Identity;
using SupportApp.ViewModels.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SupportApp.Areas.Identity.ViewComponents
{
    public class OnlineUsersViewComponent : ViewComponent
    {
        private readonly ISiteStatService _siteStatService;

        public OnlineUsersViewComponent(ISiteStatService siteStatService)
        {
            _siteStatService = siteStatService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int numbersToTake, int minutesToTake, bool showMoreItemsLink)
        {
            var usersList = await _siteStatService.GetOnlineUsersListAsync(numbersToTake, minutesToTake);
            return View(viewName: "~/Areas/Identity/Views/Shared/Components/OnlineUsers/Default.cshtml",
                        model: new OnlineUsersViewModel
                        {
                            MinutesToTake = minutesToTake,
                            NumbersToTake = numbersToTake,
                            ShowMoreItemsLink = showMoreItemsLink,
                            Users = usersList
                        });
        }
    }
}