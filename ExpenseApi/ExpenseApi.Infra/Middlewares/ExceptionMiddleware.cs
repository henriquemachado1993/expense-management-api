using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using ExpenseApi.Domain.Entities.Audit;
using ExpenseApi.Infra.Context;
using System;
using System.Threading.Tasks;

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
                LogExceptionToMongoDB(ex, mongoDBContext);

                // Devolve um erro amigável pro usuário
                await HandleExceptionAsync(context, ex.Message);
            }
        }

        private void LogExceptionToMongoDB(Exception ex, MongoDBContext mongoDBContext)
        {
            var auditLog = new AuditLog()
            {
                Id = ObjectId.GenerateNewId(),
                Message = ex.Message,
                Type = "error"
            };
            auditLog.SetDataFromObject(ex);
            var collection = mongoDBContext.Database.GetCollection<AuditLog>(typeof(AuditLog).Name);
            collection.InsertOne(auditLog);
        }

        private static Task HandleExceptionAsync(HttpContext context, string message)
        {
            // Configurar a resposta HTTP com um status code específico e o detalhe do erro
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500; // Internal Server Error

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Ocorreu um erro interno no servidor.",
                DetailedMessage = message
            };

            // Serializa a resposta em JSON
            var jsonResponse = JsonConvert.SerializeObject(response);

            // Escreve a resposta no corpo da resposta HTTP
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}
