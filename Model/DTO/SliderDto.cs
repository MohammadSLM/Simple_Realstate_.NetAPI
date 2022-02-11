using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi1.Model.DTO
{
    public class SliderDto
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Link { get; set; }
        public IFormFile Pic { get; set; }
        public string PicUrl { get; set; }
        public int Order { get; set; }
    }
    public class SliderSelectDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Link { get; set; }
        public string Pic { get; set; }
        public int Order { get; set; }
    }
}
