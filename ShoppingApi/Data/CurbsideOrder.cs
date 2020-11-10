using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Data
{
    public class CurbsideOrder
    {
        public int Id { get; set; }
        public string For { get; set; }
        public string Items { get; set; } // "1,2,3"
        public DateTime? PickupReadyAt { get; set; }
    }
}
