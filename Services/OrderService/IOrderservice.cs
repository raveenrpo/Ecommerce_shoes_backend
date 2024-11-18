using Ecommerse_shoes_backend.Data.Dto;

namespace Ecommerse_shoes_backend.Services.OrderService
{
    public interface IOrderservice
    {
        Task<string> RazorOrderCreate(long price);
        bool PaymentVerify(RazorDto razorDto);
        Task<bool> CreateOrder(int id, InputorderDto inputorderDto);

        Task<IEnumerable<OutorderDto>> GetUserOrder(int id);
        Task<IEnumerable<OutorderDto>> GetUserOrderById(int id);

        Task<IEnumerable<OrderAdminviewDto>>GetAllUserOrder();
        Task<int>TotalPurchasedProduct();
        Task<decimal> TotalRevenue();
    }
}
