using ExpenseApi.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ExpenseApi.Helper
{
    public static class ResponseHelper
    {
        public static IActionResult Handle<T>(ServiceResult<T> serviceResult)
        {
            switch (serviceResult.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.Created:
                case HttpStatusCode.Accepted:
                    return new OkObjectResult(serviceResult);
                case HttpStatusCode.NoContent:
                    return new NoContentResult();
                case HttpStatusCode.BadRequest:
                    return new BadRequestObjectResult(serviceResult);
                case HttpStatusCode.Unauthorized:
                    return new UnauthorizedObjectResult(serviceResult);
                case HttpStatusCode.NotFound:
                    return new NotFoundObjectResult(serviceResult);
                case HttpStatusCode.MethodNotAllowed:
                    return new ObjectResult(serviceResult)
                    {
                        StatusCode = (int)HttpStatusCode.MethodNotAllowed
                    };
                case HttpStatusCode.InternalServerError:
                    return new ObjectResult(serviceResult)
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError
                    };
                case HttpStatusCode.BadGateway:
                    return new ObjectResult(serviceResult)
                    {
                        StatusCode = (int)HttpStatusCode.BadGateway
                    };
                case HttpStatusCode.ServiceUnavailable:
                    return new ObjectResult(serviceResult)
                    {
                        StatusCode = (int)HttpStatusCode.ServiceUnavailable
                    };
                case HttpStatusCode.GatewayTimeout:
                    return new ObjectResult(serviceResult)
                    {
                        StatusCode = (int)HttpStatusCode.GatewayTimeout
                    };
                default:
                    return new BadRequestObjectResult(serviceResult);
            }
        }
    }
}
