using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using DNTBreadCrumb.Core;
using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupportApp.Common.GuardToolkit;
using SupportApp.Services.Contracts;
using SupportApp.Services.Identity;
using SupportApp.ViewModels;
using SupportApp.ViewModels.Identity;

namespace SupportApp.Controllers
{
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [BreadCrumb(UseDefaultRouteUrl = true, Order = 0)]
    [DisplayName("مدیریت ورژن های قفل")]
    public class LockVersionsController : Controller
    {
        private readonly ILockVersionService _lockVersionService;

        private const string SoftwareVersionNotFound = "ورژن درخواستی یافت نشد.";

        public LockVersionsController(ILockVersionService softwareVersionService)
        {
            _lockVersionService = softwareVersionService;
            _lockVersionService.CheckArgumentIsNull(nameof(_lockVersionService));
        }

        [DisplayName("ایندکس")]
        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public async Task<IActionResult> Index()
        {
            var viewModels = await _lockVersionService.GetAllAsync();
            return View(viewModels);
        }

        [HttpGet]
        [DisplayName("نمایش فرم ورژن جدید")]
        [BreadCrumb(Order = 1)]
        public async Task<IActionResult> RenderCreate()
        {
            var viewModel = new LockVersionViewModel();
            return View("Create", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [DisplayName("ایجاد یک ورژن جدید")]
        public async Task<IActionResult> Create(LockVersionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _lockVersionService.CheckExistNameAsync(viewModel.Id, viewModel.Name))
                {
                    ModelState.AddModelError(nameof(viewModel.Name), "نام وارد شده تکراری است");
                    return View(viewModel);
                }

                var result = await _lockVersionService.InsertAsync(viewModel);
                if (result)
                {
                    return RedirectToAction("Index", "LockVersions");
                }

                return View(viewModel);
            }

            return View(viewModel);
        }

        [HttpGet]
        [DisplayName("نمایش فرم ویرایش ورژن")]
        [BreadCrumb(Order = 1)]
        public async Task<IActionResult> RenderEdit(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var viewModel = await _lockVersionService.GetByIdAsync(id.GetValueOrDefault());

            if (viewModel == null)
            {
                return NotFound();
            }

            return View("Edit", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [DisplayName("ویرایش ورژن")]
        public async Task<IActionResult> Edit(LockVersionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _lockVersionService.CheckExistNameAsync(viewModel.Id, viewModel.Name))
                {
                    ModelState.AddModelError(nameof(viewModel.Name), "نام وارد شده تکراری است");
                    return View(viewModel);
                }

                var result = await _lockVersionService.UpdateAsync(viewModel);
                if (result)
                {
                    return RedirectToAction("Index", "LockVersions");
                }

                return View(viewModel);
            }

            return View(viewModel);
        }

        [AjaxOnly]
        [DisplayName("نمایش فرم حذف ورژن")]
        public async Task<IActionResult> RenderDelete([FromBody]ModelIdViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Id))
            {
                return PartialView("_Delete");
            }

            var lockVersionViewModel = await _lockVersionService.GetByIdAsync(Convert.ToInt32(model.Id));
            if (lockVersionViewModel == null)
            {
                ModelState.AddModelError("", SoftwareVersionNotFound);
                return PartialView("_Delete");
            }

            if (await _lockVersionService.CheckExistRelationAsync(lockVersionViewModel.Id))
            {
                ModelState.AddModelError("", SoftwareVersionNotFound);
                return PartialView("_Used");
            }

            return PartialView("_Delete", model: lockVersionViewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("حذف ورژن")]
        public async Task<IActionResult> Delete(LockVersionViewModel viewModel)
        {
            var lockVersionViewModel = await _lockVersionService.GetByIdAsync(viewModel.Id);
            if (lockVersionViewModel == null)
            {
                ModelState.AddModelError("", SoftwareVersionNotFound);
            }
            else
            {
                var result = await _lockVersionService.DeleteAsync(lockVersionViewModel.Id);
                if (result)
                {
                    return Json(new { success = true });
                }

                ModelState.AddModelError("", SoftwareVersionNotFound);
            }

            return PartialView("_Delete", model: viewModel);
        }

        /// <summary>
        /// For [Remote] validation
        /// </summary>
        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        [DisplayName("اعتبار سنجی نام")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ValidateName(string name, int id)
        {
            var result = await _lockVersionService.CheckExistNameAsync(id, name);
            return Json(result ? "نام وارد شده تکراری است" : "true");
        }
    }
}