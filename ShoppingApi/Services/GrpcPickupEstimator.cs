using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ShoppingApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Services
{
    public class GrpcPickupEstimator : IGenerateCurbsidePickupTimes
    {
        private readonly IOptions<PickupEstimatorConfiguration> _options;
        private readonly ILogger<GrpcPickupEstimator> _logger;

        public GrpcPickupEstimator(IOptions<PickupEstimatorConfiguration> options, ILogger<GrpcPickupEstimator> logger)
        {
            _options = options;
            _logger = logger;
        }

        public async Task<DateTime?> GetPickupDate(CurbsideOrder order)
        {
            var request = new PickupService.PickupRequest
            {
                For = order.For,
                
            };
            request.Items.AddRange(order.Items.Split(',').Select(int.Parse).ToArray());
            try
            {

                var channel = GrpcChannel.ForAddress(_options.Value.Url);
                var client = new PickupService.PickupEstimator.PickupEstimatorClient(channel);

                var response = await client.GetPickupTimeAsync(request);
                _logger.LogInformation($"Estimated a pickup date of {response.PickupTime.ToDateTime().ToShortDateString() }");
                return response.PickupTime.ToDateTime();
            }
            catch (Exception ex)
            {
                // our weak-sauce plan b.
                _logger.LogError($"Got an exception trying to get the estimated pickup for {order.Id} {ex.Message}");
                return DateTime.Now.AddDays(5);
            }
        }
    }
}
