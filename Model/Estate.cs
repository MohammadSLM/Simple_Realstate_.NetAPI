using MyApi1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.Model
{
    public class Estate
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Region { get; set; }
        public int LandArea { get; set; }
        public int BuildingArea { get; set; }
        public int Bedroom { get; set; }
        public string Peyment { get; set; }
        public int Price { get; set; }
        public double Lat { get; set; }
        public double Lang { get; set; }
        public string Desc { get; set; }
        public int Code { get; set; }
        public string VidUrl { get; set; }
        public BuildingType BuildingType { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public int ProvinceId { get; set; }
        public City Province { get; set; }
        public ICollection<EstateGallery> EstateGalleries { get; set; }
    }
}
