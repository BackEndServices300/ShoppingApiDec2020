using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Models.Products
{
    public class GetProductListSummary
    {
        public string Category { get; set; }
        public int Count { get; set; }
        public List<ProductSummaryItem> Data { get; set; }
    }
}
