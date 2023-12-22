using System.Security.Claims;

namespace ExpenseApi.Helper
{
    public static class AuthenticatedUserHelper
    {
        public static bool IsUserAuthenticated(HttpContext httpContext)
        {
            return httpContext.User.Identity?.IsAuthenticated ?? false;
        }

        public static string GetUserId(HttpContext httpContext)
        {
            return httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public static string GetUserName(HttpContext httpContext)
        {
            return httpContext.User.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static string GetUserEmail(HttpContext httpContext)
        {
            return httpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}
