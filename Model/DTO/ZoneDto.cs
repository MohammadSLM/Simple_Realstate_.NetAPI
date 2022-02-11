using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi1.Model.DTO
{
    public class ZoneDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Area { get; set; }
        public string Desc { get; set; }
        public string VidUrl { get; set; }
        public IFormFile Pic { get; set; }
        public int CityId { get; set; }
    }
    public class ZoneSelectDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Area { get; set; }
        public string Desc { get; set; }
        public string VidUrl { get; set; }
        public string Pic { get; set; }
        public int CityId { get; set; }
        public CityDetail City { get; set; }
        public int ProvinceId { get; set; }
    }
}
