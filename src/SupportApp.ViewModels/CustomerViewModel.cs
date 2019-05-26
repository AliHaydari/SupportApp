using Microsoft.AspNetCore.Mvc;
using SupportApp.ViewModels.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SupportApp.ViewModels
{
    public class CustomerViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "شماره مشتری")]
        [Remote("ValidateNumber", "Customers",
            AdditionalFields = ViewModelConstants.AntiForgeryToken + "," + nameof(Id),
            HttpMethod = "POST")]
        [StringLength(450, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} حرف باشند.", MinimumLength = 2)]
        public string Number { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "نام")]
        [StringLength(450, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} حرف باشند.", MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "نام خانوادگی")]
        [StringLength(450, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} حرف باشند.", MinimumLength = 2)]
        public string Family { get; set; }

        public string FullName => Name + " " + Family;

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "ورژن برنامه")]
        public int SoftwareVersionId { get; set; }

        public string SoftwareVersionName { get; set; }

        public string SoftwareVersionReleaseNote { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "شماره قفل")]
        [StringLength(450, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} حرف باشند.", MinimumLength = 2)]
        public string LockNumber { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "ورژن قفل")]
        [StringLength(450, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} حرف باشند.", MinimumLength = 2)]
        public string LockVersion { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "تعداد کاربر")]
        public int AccountCount { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "تعداد شرکت")]
        public int CompanyCount { get; set; }

        [Display(Name = "آدرس")]
        public string Address { get; set; }

        [Display(Name = "تلفن")]
        [StringLength(450, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} حرف باشند.", MinimumLength = 10)]
        public string Tell { get; set; }

        public int DateOfSupportEndYear { set; get; }
        public int DateOfSupportEndMonth { set; get; }
        public int DateOfSupportEndDay { set; get; }

        public string SupportEndDate => $@"{DateOfSupportEndYear}/{DateOfSupportEndMonth:00}/{DateOfSupportEndDay:00}";
    }
}
