using System.Security.Claims;

namespace ExpenseApi.Helper
{
    /// <summary>
    /// Classe responsável por ajudar a recuperar dados do usuário.
    /// </summary>
    public static class AuthenticatedUserHelper
    {
        /// <summary>
        /// Verifica se usuário está autenticado.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static bool IsUserAuthenticated(HttpContext httpContext)
        {
            return httpContext.User.Identity?.IsAuthenticated ?? false;
        }

        /// <summary>
        /// Obtém o Id do usuário logado.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static Guid GetId(HttpContext httpContext)
        {
            return Guid.Parse(httpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "");
        }

        /// <summary>
        /// Obtém o Nome do usuário logado.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static string? GetName(HttpContext httpContext)
        {
            return httpContext.User.FindFirst(ClaimTypes.Name)?.Value;
        }

        /// <summary>
        /// Obtém o Email do usuário logado.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static string? GetEmail(HttpContext httpContext)
        {
            return httpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}
