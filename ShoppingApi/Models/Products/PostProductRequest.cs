using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Models.Products
{
    public class PostProductRequest
    {
        [StringLength(200)]
        [Required]
        public string Description { get; set; }
        [Required]
        [StringLength(200)]
        public string Category { get; set; }

        [Required]
        public decimal? UnitPrice { get; set; }

    }
}
