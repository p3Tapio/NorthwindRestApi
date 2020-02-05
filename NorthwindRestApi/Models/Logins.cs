using System;
using System.Collections.Generic;

namespace NorthwindRestApi.Models
{
    public partial class Logins
    {
        public int LoginId { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
    }
}
