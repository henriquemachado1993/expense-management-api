using Microsoft.AspNetCore.Mvc;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using ExpenseApi.Helper;
using Microsoft.AspNetCore.Http;
using ExpenseApi.Domain.Patterns;
using ExpenseApi.Domain.Extensions;
using ExpenseApi.Domain.Models.Transaction;
using AutoMapper;

namespace ExpenseApi.Controllers
{
    /// <summary>
    /// API para controlar despesas.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;

        /// <summary>
        /// API para controlar despesas.
        /// </summary>
        public TransactionController(IMapper mapper, ITransactionService transactionService)
        {
            _mapper = mapper;
            _transactionService = transactionService;
        }

        /// <summary>
        /// Recupera todas as transações cadastradas
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var results = await _transactionService.GetAllAsync(AuthenticatedUserHelper.GetId(HttpContext));
            return ResponseHelper.Handle(results);
        }

        /// <summary>
        /// Recupera todas as transações cadastradas
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("get-paged")]
        public async Task<IActionResult> GetPaged(FilterTransactionRequestModel requestQueryCriteria)
        {
            var userId = AuthenticatedUserHelper.GetId(HttpContext);
            var queryCriteria = new QueryCriteria<Transaction>()
            {
                Expression = x => x.UserId == userId,
                Limit = requestQueryCriteria.Limit,
                Offset = requestQueryCriteria.Offset
            };

            if (!string.IsNullOrWhiteSpace(requestQueryCriteria.Name))
                queryCriteria.Expression = queryCriteria.Expression.AndAlso(x => x.Description.ToLower().Contains(requestQueryCriteria.Name.ToLower()));

            if (requestQueryCriteria.TransactionType != null)
                queryCriteria.Expression = queryCriteria.Expression.AndAlso(x => x.TransactionType == requestQueryCriteria.TransactionType);

            if (requestQueryCriteria.CategoryId != null)
                queryCriteria.Expression = queryCriteria.Expression.AndAlso(x => x.Category.Id == requestQueryCriteria.CategoryId);

            if (!string.IsNullOrWhiteSpace(requestQueryCriteria.CategoryName))
                queryCriteria.Expression = queryCriteria.Expression.AndAlso(x => x.Category.Name.ToLower().Contains(requestQueryCriteria.CategoryName.ToLower()));

            return ResponseHelper.Handle(await _transactionService.GetPagedAsync(queryCriteria));
        }

        /// <summary>
        /// Recupera uma transação por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _transactionService.GetByIdAsync(AuthenticatedUserHelper.GetId(HttpContext), id);
            return ResponseHelper.Handle(result);
        }

        /// <summary>
        /// Cadatra uma nova transação
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TransactionRequestModel request)
        {
            request.UserId = AuthenticatedUserHelper.GetId(HttpContext);
            var user = _mapper.Map<Transaction>(request);
            var result = await _transactionService.CreateAsync(user);

            return ResponseHelper.Handle(result);
        }

        /// <summary>
        /// Alterar uma transação
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] TransactionRequestModel request)
        {
            request.UserId = AuthenticatedUserHelper.GetId(HttpContext);
            var user = _mapper.Map<Transaction>(request);
            var result = await _transactionService.UpdateAsync(user);

            return ResponseHelper.Handle(result);
        }

        /// <summary>
        /// Deleta uma transação por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _transactionService.DeleteAsync(AuthenticatedUserHelper.GetId(HttpContext), id);
            return ResponseHelper.Handle(result);
        }
    }
}
