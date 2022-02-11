using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi1.Model
{
    public enum BuildingType
    {
        [Display(Name = "برج")]
        Tower = 0,

        [Display(Name = "آپارتمان")]
        Apartment = 1,

        [Display(Name = "زمین")]
        Land = 2,

        [Display(Name = "ویلا")]
        Villa = 3
    }
}
