using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using SupportApp.ViewModels.Identity;

namespace SupportApp.ViewModels
{
    public class SoftwareVersionViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "نام")]
        [Remote("ValidateName", "SoftwareVersions",
            AdditionalFields = ViewModelConstants.AntiForgeryToken + "," + nameof(Id),
            HttpMethod = "POST")]
        [StringLength(450, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} حرف باشند.", MinimumLength = 2)]
        public string Name { get; set; }

        [Display(Name = "تغییرات")]
        public string ReleaseNote { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }
    }
}
