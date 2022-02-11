using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi1.Model.DTO
{
    public class CityDto
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public int? ParentId { get; set; }

    }
    public class CitySelectDto
    {
        public string Title { get; set; }
        public string CityPictureUrl { get; set; }
        public int? ParentId { get; set; }
        public int? CountryId { get; set; }
    }

    public class CityDetail
    {
        public int CityId { get; set; }
        public int ProvinceId { get; set; }
        public string CityTitle { get; set; }
        public string ProvinceTitle { get; set; }
    }
}
