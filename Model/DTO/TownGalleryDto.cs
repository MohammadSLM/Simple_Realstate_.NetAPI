using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi1.Model.DTO
{
    public class TownGalleryDto
    {
        public IFormFile Pic { get; set; }
        public int TownId { get; set; }
    }
}
