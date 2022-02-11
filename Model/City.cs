using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.Model
{
    public class City
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? ParentId { get; set; }
        public ICollection<Estate> Estates { get; set; }
        public ICollection<Town> Towns { get; set; }
        public ICollection<Zone> Zones { get; set; }


    }
}
