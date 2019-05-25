using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using DNTBreadCrumb.Core;
using DNTCommon.Web.Core;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SupportApp.Common.GuardToolkit;
using SupportApp.Services.Contracts;
using SupportApp.Services.Identity;
using SupportApp.ViewModels;
using SupportApp.ViewModels.Identity;

namespace SupportApp.Controllers
{
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [BreadCrumb(UseDefaultRouteUrl = true, Order = 0)]
    [DisplayName("مدیریت ورژن های برنامه")]
    public class SoftwareVersionsController : Controller
    {
        private readonly ISoftwareVersionService _softwareVersionService;

        private const string SoftwareVersionNotFound = "ورژن درخواستی یافت نشد.";

        public SoftwareVersionsController(ISoftwareVersionService softwareVersionService)
        {
            _softwareVersionService = softwareVersionService;
            _softwareVersionService.CheckArgumentIsNull(nameof(_softwareVersionService));
        }

        [DisplayName("ایندکس")]
        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public async Task<IActionResult> Index()
        {
            var viewModels = await _softwareVersionService.GetAllAsync();
            return View(viewModels);
        }

        [HttpGet]
        [DisplayName("ایجاد یک ورژن جدید")]
        [BreadCrumb(Order = 1)]
        public async Task<IActionResult> Create()
        {
            var viewModel = new SoftwareVersionViewModel();
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SoftwareVersionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _softwareVersionService.CheckExistNameAsync(viewModel.Id, viewModel.Name))
                {
                    ModelState.AddModelError(nameof(viewModel.Name), "نام وارد شده تکراری است");
                    return View(viewModel);
                }

                var result = await _softwareVersionService.InsertAsync(viewModel);
                if (result)
                {
                    return RedirectToAction("Index", "SoftwareVersions");
                }

                return View(viewModel);
            }

            return View(viewModel);
        }

        [HttpGet]
        [DisplayName("ویرایش ورژن")]
        [BreadCrumb(Order = 1)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var viewModel = await _softwareVersionService.GetByIdAsync(id.GetValueOrDefault());

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SoftwareVersionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _softwareVersionService.CheckExistNameAsync(viewModel.Id, viewModel.Name))
                {
                    ModelState.AddModelError(nameof(viewModel.Name), "نام وارد شده تکراری است");
                    return View(viewModel);
                }

                var result = await _softwareVersionService.UpdateAsync(viewModel);
                if (result)
                {
                    return RedirectToAction("Index", "SoftwareVersions");
                }

                return View(viewModel);
            }

            return View(viewModel);
        }

        [AjaxOnly]
        public async Task<IActionResult> RenderDelete([FromBody]ModelIdViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Id))
            {
                return PartialView("_Delete");
            }

            var softwareVersionViewModel = await _softwareVersionService.GetByIdAsync(Convert.ToInt32(model.Id));
            if (softwareVersionViewModel == null)
            {
                ModelState.AddModelError("", SoftwareVersionNotFound);
                return PartialView("_Delete");
            }

            //if (await _softwareVersionService.CheckExistRelationAsync(softwareVersionViewModel.Id))
            //{
            //    ModelState.AddModelError("", SoftwareVersionNotFound);
            //    return PartialView("_Used");
            //}

            return PartialView("_Delete", model: softwareVersionViewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(SoftwareVersionViewModel viewModel)
        {
            var softwareVersionViewModel = await _softwareVersionService.GetByIdAsync(viewModel.Id);
            if (softwareVersionViewModel == null)
            {
                ModelState.AddModelError("", SoftwareVersionNotFound);
            }
            else
            {
                var result = await _softwareVersionService.DeleteAsync(softwareVersionViewModel.Id);
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
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ValidateName(string name, int id)
        {
            var result = await _softwareVersionService.CheckExistNameAsync(id, name);
            return Json(result ? "نام وارد شده تکراری است" : "true");
        }
    }
}