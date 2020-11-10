using Microsoft.EntityFrameworkCore;
using ShoppingApi.Data;
using ShoppingApi.Models.Curbside;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Services
{
    public class EfSqlSynchCurbside : ICurbsideCommands, ICurbsideLookups
    {
        protected readonly ShoppingDataContext _context;

        public EfSqlSynchCurbside(ShoppingDataContext context)
        {
            _context = context;
        }

        public async Task<GetCurbsideResponse> GetById(int id)
        {
            var response = await _context.CurbsideOrders
                 .SingleOrDefaultAsync(c => c.Id == id);
            if (response != null)
            {
                
                return new GetCurbsideResponse
                {
                    Id = response.Id,
                    For = response.For,
                    Items = response.Items.Split(',').Select(int.Parse).ToArray(),
                    PickupReadyAt = response.PickupReadyAt
                };
            }
            else
            {
                return null;
            }
            
        }

        public virtual async Task<GetCurbsideResponse> PlaceOrder(PostCurbsideRequest request)
        {
            // do the "processing" (??)
            await Task.Delay(1000 * request.Items.Length);
            // produce a pickupDateTime for it.

            // Save it to the database, 
            var order = new CurbsideOrder
            {
                For = request.For,
                Items = String.Join(",", request.Items),
                PickupReadyAt = DateTime.Now.AddHours(new Random().Next(2, 5))
            };
            _context.CurbsideOrders.Add(order);
            await _context.SaveChangesAsync();
            // map it, return it.

            return new GetCurbsideResponse
            {
                Id = order.Id,
                For = order.For,
                Items = order.Items.Split(',').Select(int.Parse).ToArray(),
                PickupReadyAt = order.PickupReadyAt.Value
            };
        }
    }
}
