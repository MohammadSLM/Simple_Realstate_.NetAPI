using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.Model
{
    public class TownGallery
    {
        public int Id { get; set; }
        public string PicUrl { get; set; }
        public int TownId { get; set; }
        public Town Town { get; set; }
    }
}
