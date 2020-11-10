using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Models.Curbside
{
    public class PostCurbsideRequest
    {
        public string For { get; set; }
        public int[] Items { get; set; }
    }
}
