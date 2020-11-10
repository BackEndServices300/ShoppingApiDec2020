using ShoppingApi.Models.Curbside;
using System.Threading.Tasks;

namespace ShoppingApi
{
    public interface ICurbsideLookups
    {
        Task<GetCurbsideResponse> GetById(int id);
    }
}