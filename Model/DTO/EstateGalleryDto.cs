using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi1.Model.DTO
{
    public class EstateGalleryDto
    {
        public IFormFile Pic { get; set; }
        public int EstateId { get; set; }
    }
}
