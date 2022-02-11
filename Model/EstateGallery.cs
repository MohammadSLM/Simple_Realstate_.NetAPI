using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.Model
{
    public class EstateGallery
    {
        public int Id { get; set; }
        public string PicUrl { get; set; }
        public int EstateId { get; set; }
        public Estate Estate { get; set; }
    }
}
