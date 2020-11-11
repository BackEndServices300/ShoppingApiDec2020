using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Services
{
    public class PickupEstimatorConfiguration
    {
        public readonly string SectionName = "PickupEstimator";
        public string Url { get; set; }
    }
}
