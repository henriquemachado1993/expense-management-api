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

        public TransactionController(IMapper mapper, ITransactionService transactionService)
        {
            _mapper = mapper;
            _transactionService = transactionService;
        }

        /// <summary>
        /// Recupera todas as transações cadastradas
        /// </summary>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var results = await _transactionService.GetAllAsync(AuthenticatedUserHelper.GetUserId(HttpContext));
            return ResponseHelper.Handle(results);
        }

        /// <summary>
        /// Recupera todas as transações cadastradas
        /// </summary>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpPost("get-paged")]
        public async Task<IActionResult> GetPaged(FilterTransactionRequestModel requestQueryCriteria)
        {
            var userId = AuthenticatedUserHelper.GetUserId(HttpContext);
            var queryCriteria = new QueryCriteria<Transaction>()
            {
                Expression = x => x.UserId == userId,
                Limit = requestQueryCriteria.Limit,
                Offset = requestQueryCriteria.Offset
            };

            if (!string.IsNullOrWhiteSpace(requestQueryCriteria.Name))
                queryCriteria.Expression = queryCriteria.Expression.AndAlso(x => x.Description.ToLower().Contains(requestQueryCriteria.Name.ToLower()));

            if (!string.IsNullOrWhiteSpace(requestQueryCriteria.TransactionType))
                queryCriteria.Expression = queryCriteria.Expression.AndAlso(x => x.TransactionType.ToLower() == requestQueryCriteria.TransactionType.ToLower());

            return ResponseHelper.Handle(await _transactionService.GetPagedAsync(queryCriteria));
        }

        /// <summary>
        /// Recupera uma transação por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _transactionService.GetByIdAsync(AuthenticatedUserHelper.GetUserId(HttpContext), id);
            return ResponseHelper.Handle(result);
        }

        /// <summary>
        /// Cadatra uma nova transação
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TransactionRequestModel request)
        {
            request.UserId = AuthenticatedUserHelper.GetUserId(HttpContext);
            var user = _mapper.Map<Transaction>(request);
            var result = await _transactionService.CreateAsync(user);

            return ResponseHelper.Handle(result);
        }

        /// <summary>
        /// Alterar uma transação
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] TransactionRequestModel request)
        {
            request.UserId = AuthenticatedUserHelper.GetUserId(HttpContext);
            var user = _mapper.Map<Transaction>(request);
            var result = await _transactionService.UpdateAsync(user);

            return ResponseHelper.Handle(result);
        }

        /// <summary>
        /// Deleta uma transação por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _transactionService.DeleteAsync(AuthenticatedUserHelper.GetUserId(HttpContext), id);
            return ResponseHelper.Handle(result);
        }
    }
}
