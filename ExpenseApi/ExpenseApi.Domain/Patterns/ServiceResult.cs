using ExpenseApi.Domain.Enums;
using System.Net;

namespace ExpenseApi.Domain.Patterns
{
    /// <summary>
    /// Classe usada para transitar entre serviços e controller
    /// </summary>    
    public class ServiceResult<T>
    {
        #region Constructor

        public ServiceResult()
        {
            Messages = new List<MessageResult>();
        }

        public ServiceResult(T data, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            Data = data;
            Messages = new List<MessageResult>();
            StatusCode = statusCode;
        }

        #endregion

        public T Data { get; set; }
        public List<MessageResult> Messages { get; set; }
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

        public bool IsValid
        {
            get
            {
                return Messages == null || !Messages.Any(x => x.Type == MessageType.Error.ToString());
            }
        }

        #region Factory

        public static ServiceResult<T> CreateValidResult(T model)
        {
            return new ServiceResult<T>(model);
        }

        public static ServiceResult<T> CreateValidResult()
        {
            return new ServiceResult<T>(default);
        }

        public static ServiceResult<T> CreateInvalidResult(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            var result = new ServiceResult<T>(default, statusCode);
            result.Messages = AddError(message);
            result.StatusCode = statusCode;
            return result;
        }

        public static ServiceResult<T> CreateInvalidResult(T model, string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            var result = new ServiceResult<T>();
            result.Data = model;
            result.Messages = AddError(message);
            result.StatusCode = statusCode;

            return result;
        }

        public static ServiceResult<T> CreateInvalidResult(List<MessageResult> message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            var bo = new ServiceResult<T>
            {
                Messages = message,
                StatusCode = statusCode
            };
            return bo;
        }

        public static ServiceResult<T> CreateInvalidResult(T model, string message, Exception exception, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            var result = new ServiceResult<T>();
            result.Data = model;
            if(exception != null)
                result.Messages = AddError(message, exception);
            result.StatusCode = statusCode;
            return result;
        }

        public static ServiceResult<T> CreateInvalidResult(Exception exception, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            var result = new ServiceResult<T>();
            if(exception != null)
                result.Messages = AddError(exception);
            result.StatusCode = statusCode;
            return result;
        }

        public static ServiceResult<T> CreateInvalidResult(Exception exception, string message = "Ocorreu um erro.", HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            var result = new ServiceResult<T>();
            if(exception != null)
                result.Messages = AddError(message, exception);
            result.StatusCode = statusCode;
            return result;
        }

        public static ServiceResult<T> CreateInvalidResult(IEnumerable<string> errors, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            var result = new ServiceResult<T>();
            result.Messages = AddError(errors);
            result.StatusCode = statusCode;
            return result;
        }

        public void WithErrors(params string[] errors)
        {
            if (errors != null && errors.Any())
            {
                Messages.AddRange(AddError(errors));
            }
        }

        public void WithErrors(List<MessageResult> errors)
        {
            if (errors != null && errors.Any())
            {
                Messages.AddRange(errors);
            }
        }

        public void WithErrors(MessageResult errors)
        {
            if (errors != null)
            {
                Messages.Add(errors);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageError">Mensagem de erro</param>
        /// <param name="typeCustom">Tipo customizado em HTML para usar no frontend</param>
        public void WithErrors(string messageError, string typeCustom)
        {
            if (!string.IsNullOrEmpty(messageError))
            {
                Messages.Add(new MessageResult() { 
                    Message = messageError , 
                    Type = MessageType.Error.ToString(),
                    TypeCustom = typeCustom
                });
            }
        }

        public void WithErrors(Exception exception)
        {
            if (exception != null)
            {
                Messages.AddRange(AddError(exception.Message));
            }
        }

        public void WithSucess(params string[] message)
        {
            if (message != null && message.Any())
            {
                Messages.AddRange(AddSucess(message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">Mensagem de sucesso</param>
        /// <param name="typeCustom">Usado para algo em HTML no frontend</param>
        public void WithSucess(string message, string typeCustom = "")
        {
            if (message != null && message.Any())
            {
                Messages.AddRange(AddSucess(message, typeCustom));
            }
        }

        private static List<MessageResult> AddSucess(string message, string typeCustom = "")
        {
            var messageResult = new List<MessageResult>();
            messageResult.Add(new MessageResult()
            {
                Key = Guid.NewGuid().ToString(),
                Message = message,
                Type = MessageType.Success.ToString(),
                TypeCustom = typeCustom
            });
            return messageResult;
        }

        private static List<MessageResult> AddSucess(IEnumerable<string> message)
        {
            var messageResult = new List<MessageResult>();
            foreach (var item in message)
            {
                messageResult.Add(new MessageResult()
                {
                    Key = Guid.NewGuid().ToString(),
                    Message = item,
                    Type = MessageType.Success.ToString()
                });
            }
            return messageResult;
        }

        private static List<MessageResult> AddError(string message, Exception? exception = null)
        {
            var messageResult = new List<MessageResult>();
            messageResult.Add(new MessageResult()
            {
                Key = Guid.NewGuid().ToString(),
                Message = message,
                Type = MessageType.Error.ToString()
            });
            if (exception != null)
            {
                messageResult.Add(new MessageResult()
                {
                    Key = Guid.NewGuid().ToString(),
                    Message = exception.Message,
                    Type = MessageType.Error.ToString()
                });
            }
            return messageResult;
        }

        private static List<MessageResult> AddError(Exception? exception = null)
        {
            var messageResult = new List<MessageResult>();
            if (exception != null)
            {
                messageResult.Add(new MessageResult()
                {
                    Key = Guid.NewGuid().ToString(),
                    Message = exception.Message,
                    Type = MessageType.Error.ToString()
                });
            }
            return messageResult;
        }

        private static List<MessageResult> AddError(IEnumerable<string> errors, Exception? exception = null)
        {
            var messageResult = new List<MessageResult>();
            foreach (var item in errors)
            {
                messageResult.Add(new MessageResult()
                {
                    Key = Guid.NewGuid().ToString(),
                    Message = item,
                    Type = MessageType.Error.ToString()
                });
            }

            if (exception != null)
            {
                messageResult.Add(new MessageResult()
                {
                    Key = Guid.NewGuid().ToString(),
                    Message = exception.Message,
                    Type = MessageType.Error.ToString()
                });
            }
            return messageResult;
        }

        #endregion
    }
}
