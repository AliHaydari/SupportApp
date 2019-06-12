using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SupportApp.ViewModels
{
    public class RequestTypeViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "نام")]
        [StringLength(450, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} حرف باشند.", MinimumLength = 2)]
        public string Name { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }
    }
}
