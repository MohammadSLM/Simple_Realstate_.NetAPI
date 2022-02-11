using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi1.Model.DTO
{
    public class ProjectDto
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string VidLink { get; set; }

    }
}
