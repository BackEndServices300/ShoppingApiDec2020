using ShoppingApi.Data;
using ShoppingApi.Models.Curbside;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Services
{
    public class EfSqlAsyncCurbside : EfSqlSynchCurbside
    {
        private readonly CurbsideChannel _channel;
        public EfSqlAsyncCurbside(ShoppingDataContext context, CurbsideChannel channel) : base(context)
        {
            _channel = channel;
        }

        public override async Task<GetCurbsideResponse> PlaceOrder(PostCurbsideRequest request)
        {
           

            // Save it to the database, 
            var order = new CurbsideOrder
            {
                For = request.For,
                Items = String.Join(",", request.Items),
                PickupReadyAt = null
            };
            _context.CurbsideOrders.Add(order);
            await _context.SaveChangesAsync();
            // map it, return it.
            if(await _channel.AddCurbside(new CurbsideChannelRequest { OrderId = order.Id }))
            {
                // it was able to write it.
            } else
            {
                // it wasn't
            }
            return new GetCurbsideResponse
            {
                Id = order.Id,
                For = order.For,
                Items = order.Items.Split(',').Select(int.Parse).ToArray(),
                PickupReadyAt = null
            };
        }
    }
}
