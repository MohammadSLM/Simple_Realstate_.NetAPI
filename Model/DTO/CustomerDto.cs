using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi1.Model.DTO
{
    public class CustomerDto
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Desc { get; set; }
        public string VidUrl { get; set; }
    }
}
