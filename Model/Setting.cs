using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi1.Model
{
    public class Setting
    {
        public int Id { get; set; }
        public string BannerPicUrl1 { get; set; }
        public string BannerPicUrl2 { get; set; }
        public string CustomerCount { get; set; }
        public string ProjectCount { get; set; }
        public string TownCount { get; set; }
        public string ZoneCount { get; set; }
    }
}
