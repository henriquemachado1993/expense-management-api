using System.Security.Claims;

namespace ExpenseApi.Helper
{
    public static class AuthenticatedUserHelper
    {
        public static bool IsUserAuthenticated(HttpContext httpContext)
        {
            return httpContext.User.Identity?.IsAuthenticated ?? false;
        }

        public static Guid GetId(HttpContext httpContext)
        {
            return Guid.Parse(httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }

        public static string GetName(HttpContext httpContext)
        {
            return httpContext.User.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static string GetEmail(HttpContext httpContext)
        {
            return httpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}
