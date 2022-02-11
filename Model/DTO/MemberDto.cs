﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.Model.DTO
{
    public class MemberDto
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Position { get; set; }
        public string PhoneNumber { get; set; }
        public string WhatsApp { get; set; }
        public string Instagram { get; set; }
        public IFormFile Pic { get; set; }

    }
    public class MemberSelectDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public string PhoneNumber { get; set; }
        public string WhatsApp { get; set; }
        public string Instagram { get; set; }
        public string Pic { get; set; }
    }
}
