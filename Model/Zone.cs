using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.Model
{
    public class Zone
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Area { get; set; }
        public string Desc { get; set; }
        public string PicUrl { get; set; }
        public string VidUrl { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
    }
}
