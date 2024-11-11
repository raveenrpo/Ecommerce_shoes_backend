using Ecommerse_shoes_backend.Data.Dto;

namespace Ecommerse_shoes_backend.Services.OrderService
{
    public interface IOrderservice
    {
        Task<string> OrderCreate(long price);
        bool PaymentVerify(RazorDto razorDto);
        Task<IEnumerable<OutorderDto>> GetUserOrder(int id);
        Task<IEnumerable<OutorderDto>>GetAllUserOrder();
        Task<int>TotalPurchaseByProduct(int id);
        Task<decimal> TotalRevenue();
        Task<bool> CreateOrder(int id,InputorderDto inputorderDto);
    }
}
