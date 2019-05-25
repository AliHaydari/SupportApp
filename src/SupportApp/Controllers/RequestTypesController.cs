using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DNTBreadCrumb.Core;
using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupportApp.Common.GuardToolkit;
using SupportApp.Common.IdentityToolkit;
using SupportApp.Services.Contracts;
using SupportApp.Services.Identity;
using SupportApp.ViewModels;
using SupportApp.ViewModels.Identity;

namespace SupportApp.Controllers
{
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [BreadCrumb(UseDefaultRouteUrl = true, Order = 0)]
    [DisplayName("مدیریت نوع درخواست ها")]
    public class RequestTypesController : Controller
    {
        private const string RequestTypeNotFound = "نوع درخواست درخواستی یافت نشد.";
        private const int DefaultPageSize = 7;

        private readonly IRequestTypeService _requestTypeService;

        public RequestTypesController(IRequestTypeService requestTypeService)
        {
            _requestTypeService = requestTypeService;
            _requestTypeService.CheckArgumentIsNull(nameof(_requestTypeService));
        }

        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public async Task<IActionResult> Index()
        {
            var requestTypeViewModels = await _requestTypeService.GetAllAsync();
            return View(requestTypeViewModels);
        }

        [AjaxOnly]
        public async Task<IActionResult> RenderRequestType([FromBody]ModelIdViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Id))
            {
                return PartialView("_Create", new RequestTypeViewModel());
            }

            var requestTypeViewModel = await _requestTypeService.GetByIdAsync(Convert.ToInt32(model.Id));
            if (requestTypeViewModel == null)
            {
                ModelState.AddModelError("", RequestTypeNotFound);
                return PartialView("_Create");
            }

            return PartialView("_Create", requestTypeViewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRequestType(RequestTypeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var requestTypeViewModel = await _requestTypeService.GetByIdAsync(viewModel.Id);
                if (requestTypeViewModel == null)
                {
                    ModelState.AddModelError("", RequestTypeNotFound);
                }
                else
                {
                    var result = await _requestTypeService.UpdateAsync(viewModel);
                    if (result)
                    {
                        return Json(new { success = true });
                    }

                    ModelState.AddModelError("", RequestTypeNotFound);
                }
            }

            return PartialView("_Create", model: viewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRequestType(RequestTypeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _requestTypeService.InsertAsync(viewModel);
                if (result)
                {
                    return Json(new { success = true });
                }

                ModelState.AddModelError("", RequestTypeNotFound);
            }

            return PartialView("_Create", model: viewModel);
        }

        [AjaxOnly]
        public async Task<IActionResult> RenderDeleteRequestType([FromBody]ModelIdViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Id))
            {
                return PartialView("_Delete");
            }

            var requestTypeViewModel = await _requestTypeService.GetByIdAsync(Convert.ToInt32(model.Id));
            if (requestTypeViewModel == null)
            {
                ModelState.AddModelError("", RequestTypeNotFound);
                return PartialView("_Delete");
            }

            return PartialView("_Delete", model: requestTypeViewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(RequestTypeViewModel viewModel)
        {
            var requestTypeViewModel = await _requestTypeService.GetByIdAsync(viewModel.Id);
            if (requestTypeViewModel == null)
            {
                ModelState.AddModelError("", RequestTypeNotFound);
            }
            else
            {
                var result = await _requestTypeService.DeleteAsync(requestTypeViewModel.Id);
                if (result)
                {
                    return Json(new { success = true });
                }

                ModelState.AddModelError("", RequestTypeNotFound);
            }

            return PartialView("_Delete", model: viewModel);
        }
    }
}