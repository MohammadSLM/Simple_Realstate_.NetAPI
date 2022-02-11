using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.Model
{
    public class Slider
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Link { get; set; }
        public string PicUrl { get; set; }
        public int Order { get; set; }

    }
}
