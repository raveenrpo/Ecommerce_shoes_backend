namespace Ecommerse_shoes_backend.Middleware
{
    public interface IJwtTokengetId
    {
        int GetUserId(string token);
    }
}
