using ShoppingApi.Models.Curbside;
using System.Threading.Tasks;

namespace ShoppingApi
{
    public interface ICurbsideCommands
    {
        Task<GetCurbsideResponse> PlaceOrder(PostCurbsideRequest request);
    }
}