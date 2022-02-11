using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi1.Model.DTO
{
    public class IndexDto
    {
        public int ZonesCount{ get; set; }
        public int EstatesCount { get; set; }
        public int TownsCount { get; set; }
        public int MembersCount { get; set; }
        public int ProjectsCount { get; set; }
        public int CustomersCount { get; set; }
        public int SlidersCount { get; set; }
        public List<ZoneSelectDto> Zones5 { get; set; }
        public List<EstateSelectDto> Estate5 { get; set; }
        public List<TownSelectDto> Town5 { get; set; }
    }
}
