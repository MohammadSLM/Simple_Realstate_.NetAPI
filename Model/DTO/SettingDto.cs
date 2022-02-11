using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi1.Model.DTO
{
    public class SettingDto
    {
        public int Id { get; set; }
        public IFormFile BannerPicUrl1 { get; set; }
        public string BannerPicUrl1Url { get; set; }
        public IFormFile BannerPicUrl2 { get; set; }
        public string BannerPicUrl2Url { get; set; }
        public string CustomerCount { get; set; }
        public string ProjectCount { get; set; }
        public string TownCount { get; set; }
        public string ZoneCount { get; set; }
    }
}
