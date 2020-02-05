using System;
using System.Collections.Generic;

namespace NorthwindRestApi.Models
{
    public partial class Region
    {
        public Region()
        {
            Shippers = new HashSet<Shippers>();
            Territories = new HashSet<Territories>();
        }

        public int RegionId { get; set; }
        public string RegionDescription { get; set; }

        public ICollection<Shippers> Shippers { get; set; }
        public ICollection<Territories> Territories { get; set; }
    }
}
