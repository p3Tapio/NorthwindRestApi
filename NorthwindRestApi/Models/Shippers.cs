using System;
using System.Collections.Generic;

namespace NorthwindRestApi.Models
{
    public partial class Shippers
    {
        public Shippers()
        {
            Orders = new HashSet<Orders>();
        }

        public int ShipperId { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public int? RegionId { get; set; }

        public Region Region { get; set; }
        public ICollection<Orders> Orders { get; set; }
    }
}
