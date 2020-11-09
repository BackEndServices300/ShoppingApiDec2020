using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ShoppingApi.Data;
using ShoppingApi.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Services
{
    public class EfSqlShopping : ILookupProducts
    {
        private readonly ShoppingDataContext _context;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _config;

        public EfSqlShopping(ShoppingDataContext context, IMapper mapper, MapperConfiguration config)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
        }

        public async Task<GetProductsResponse> GetSummary()
        {
            var response = new GetProductsResponse();

            var countInInventory = await _context.Products.CountAsync(p => p.InInventory);
            var countOutOfInventory = await _context.Products.CountAsync(p => !p.InInventory);

            response.NumberOfProductsInInventory = countInInventory;
            response.NumberOfProductsOutOfStock = countOutOfInventory;
            response.NumberOfProductsAddedToday = new Random().Next(12, 300); //HA!
            return response;
        }

        public async Task<GetProductListSummary> GetSummaryList(string category)
        {
            var list = await _context.GetItemsFromCategory(category)
                .ProjectTo<ProductSummaryItem>(_config)
                .ToListAsync();

            var response = new GetProductListSummary
            {
                Data = list,
                Category = category,
                Count = list.Count()
            };
            return response;
        }
    }
}
