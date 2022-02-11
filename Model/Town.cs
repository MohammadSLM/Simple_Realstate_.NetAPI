using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.Model
{
    public class Town
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Peyment { get; set; }
        public string UtmPicUrl { get; set; }
        public double Lat { get; set; }
        public double Lang { get; set; }
        public string VidUrl { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public int ProvinceId { get; set; }
        public City Province { get; set; }
        public ICollection<TownGallery> TownGalleries { get; set; }
    }
}
