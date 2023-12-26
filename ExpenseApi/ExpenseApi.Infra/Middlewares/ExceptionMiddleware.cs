using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using ExpenseApi.Domain.Entities.Audit;
using ExpenseApi.Infra.Context;
using System;
using System.Threading.Tasks;
using ExpenseApi.Domain.Patterns;
using ExpenseApi.Domain.Enums;
using System.Net;

namespace ExpenseApi.Infra.Middlewares
{
    public  class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, MongoDBContext mongoDBContext)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // grava no mongoDB
                await LogExceptionToMongoDB(mongoDBContext, ex);

                // Devolve um erro amigável pro usuário
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task LogExceptionToMongoDB(MongoDBContext mongoDBContext, Exception ex)
        {
            var auditLog = new AuditLog(Guid.NewGuid(), MessageType.Error.ToString(), ex.Message, DateTime.Now, GetDataByException(ex));
            
            var collection = mongoDBContext.Database.GetCollection<AuditLog>(typeof(AuditLog).Name);
            return collection.InsertOneAsync(auditLog);
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // Configurar a resposta HTTP com um status code específico e o detalhe do erro
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500; // Internal Server Error

            var response = ServiceResult<object>.CreateInvalidResult(GetDataByException(ex), ex.Message, HttpStatusCode.InternalServerError);

            // Serializa a resposta em JSON
            var jsonResponse = JsonConvert.SerializeObject(response);

            // Escreve a resposta no corpo da resposta HTTP
            return context.Response.WriteAsync(jsonResponse);
        }

        private static object GetDataByException(Exception ex)
        {
            return new
            {
                Message = ex.Message,
                StackTraceString = ex.StackTrace
            };
        }
    }
}
