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
using SupportApp.Common.IdentityToolkit;
using SupportApp.Services.Contracts;
using SupportApp.Services.Identity;
using SupportApp.ViewModels;
using SupportApp.ViewModels.Identity;

namespace SupportApp.Controllers
{
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [BreadCrumb(UseDefaultRouteUrl = true, Order = 0)]
    [DisplayName("مدیریت مشتری ها")]
    public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ISoftwareVersionService _softwareVersionService;
        private readonly ILockVersionService _lockVersionService;

        private const string CustomerNotFound = "مشتری درخواستی یافت نشد.";

        public CustomersController(ICustomerService customerService, ISoftwareVersionService softwareVersionService, ILockVersionService lockVersionService)
        {
            _customerService = customerService;
            _customerService.CheckArgumentIsNull(nameof(_customerService));

            _softwareVersionService = softwareVersionService;
            _softwareVersionService.CheckArgumentIsNull(nameof(_softwareVersionService));

            _lockVersionService = lockVersionService;
            _lockVersionService.CheckArgumentIsNull(nameof(_lockVersionService));
        }

        [DisplayName("ایندکس")]
        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public async Task<IActionResult> Index()
        {
            var viewModels = await _customerService.GetAllAsync();
            return View(viewModels);
        }

        [HttpGet]
        [DisplayName("نمایش فرم ایجاد یک مشتری جدید")]
        [BreadCrumb(Order = 1)]
        public async Task<IActionResult> RenderCreate()
        {
            var pDate = DateTime.Now.ToPersianYearMonthDay();
            var viewModel = new CustomerViewModel()
            {
                DateOfSupportEndYear = pDate.Year,
                DateOfSupportEndMonth = pDate.Month,
                DateOfSupportEndDay = pDate.Day,
            };

            await PopulateSoftwareVersionsAsync(null);
            await PopulateLockVersionsAsync(null);

            return View("Create", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [DisplayName("ایجاد یک مشتری جدید")]
        public async Task<IActionResult> Create(CustomerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _customerService.CheckExistNumberAsync(viewModel.Id, viewModel.Number))
                {
                    ModelState.AddModelError(nameof(viewModel.Number), "شماره مشتری وارد شده تکراری است");
                    await PopulateSoftwareVersionsAsync(viewModel.SoftwareVersionId);
                    await PopulateLockVersionsAsync(viewModel.LockVersionId);
                    return View(viewModel);
                }

                var result = await _customerService.InsertAsync(viewModel);
                if (result)
                {
                    return RedirectToAction("Index", "Customers");
                }

                await PopulateSoftwareVersionsAsync(viewModel.SoftwareVersionId);
                await PopulateLockVersionsAsync(viewModel.LockVersionId);
                return View(viewModel);
            }

            await PopulateSoftwareVersionsAsync(viewModel.SoftwareVersionId);
            await PopulateLockVersionsAsync(viewModel.LockVersionId);
            return View(viewModel);
        }

        [HttpGet]
        [DisplayName("نمایش فرم ویرایش مشتری")]
        [BreadCrumb(Order = 1)]
        public async Task<IActionResult> RenderEdit(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var viewModel = await _customerService.GetByIdAsync(id.GetValueOrDefault());

            if (viewModel == null)
            {
                return NotFound();
            }

            await PopulateSoftwareVersionsAsync(viewModel.SoftwareVersionId);
            await PopulateLockVersionsAsync(viewModel.LockVersionId);

            return View("Edit", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [DisplayName("ویرایش مشتری")]
        public async Task<IActionResult> Edit(CustomerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _customerService.CheckExistNumberAsync(viewModel.Id, viewModel.Number))
                {
                    ModelState.AddModelError(nameof(viewModel.Number), "شماره مشتری وارد شده تکراری است");
                    await PopulateSoftwareVersionsAsync(viewModel.SoftwareVersionId);
                    await PopulateLockVersionsAsync(viewModel.LockVersionId);
                    return View(viewModel);
                }

                var result = await _customerService.UpdateAsync(viewModel);
                if (result)
                {
                    return RedirectToAction("Index", "Customers");
                }

                await PopulateSoftwareVersionsAsync(viewModel.SoftwareVersionId);
                await PopulateLockVersionsAsync(viewModel.LockVersionId);
                return View(viewModel);
            }

            await PopulateSoftwareVersionsAsync(viewModel.SoftwareVersionId);
            await PopulateLockVersionsAsync(viewModel.LockVersionId);
            return View(viewModel);
        }

        [AjaxOnly]
        [DisplayName("نمایش فرم حذف مشتری")]
        public async Task<IActionResult> RenderDelete([FromBody]ModelIdViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Id))
            {
                return PartialView("_Delete");
            }

            var customerViewModel = await _customerService.GetByIdAsync(Convert.ToInt32(model.Id));
            if (customerViewModel == null)
            {
                ModelState.AddModelError("", CustomerNotFound);
                return PartialView("_Delete");
            }

            if (await _customerService.CheckExistRelationAsync(customerViewModel.Id))
            {
                ModelState.AddModelError("", CustomerNotFound);
                return PartialView("_Used");
            }

            return PartialView("_Delete", model: customerViewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("حذف مشتری")]
        public async Task<IActionResult> Delete(CustomerViewModel viewModel)
        {
            var customerViewModel = await _customerService.GetByIdAsync(viewModel.Id);
            if (customerViewModel == null)
            {
                ModelState.AddModelError("", CustomerNotFound);
            }
            else
            {
                var result = await _customerService.DeleteAsync(customerViewModel.Id);
                if (result)
                {
                    return Json(new { success = true });
                }

                ModelState.AddModelError("", CustomerNotFound);
            }

            return PartialView("_Delete", model: viewModel);
        }

        private async Task PopulateSoftwareVersionsAsync(int? softwareVersionId)
        {
            var data = await _softwareVersionService.GetAllAsync();

            var selectList = new SelectList(data,
                nameof(SoftwareVersionViewModel.Id),
                nameof(SoftwareVersionViewModel.Name),
                softwareVersionId.GetValueOrDefault());

            ViewBag.PopulateSoftwareVersions = selectList;
        }

        private async Task PopulateLockVersionsAsync(int? lockVersionId)
        {
            var data = await _lockVersionService.GetAllAsync();

            var selectList = new SelectList(data,
                nameof(LockVersionViewModel.Id),
                nameof(SoftwareVersionViewModel.Name),
                lockVersionId.GetValueOrDefault());

            ViewBag.PopulateLockVersions = selectList;
        }

        /// <summary>
        /// For [Remote] validation
        /// </summary>
        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        [DisplayName("اعتبار سنجی شماره مشتری")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ValidateNumber(string number, int id)
        {
            var result = await _customerService.CheckExistNumberAsync(id, number);
            return Json(result ? "شماره مشتری وارد شده تکراری است" : "true");
        }
    }
}