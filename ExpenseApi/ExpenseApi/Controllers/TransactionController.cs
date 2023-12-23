using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Models;
using Microsoft.AspNetCore.Authorization;
using ExpenseApi.Helper;
using Microsoft.AspNetCore.Http;

namespace ExpenseApi.Controllers
{
    /// <summary>
    /// API para controlar despesas.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
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
        /// Recupera uma transação por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
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
            // TODO: colocar automapper.
            var result = await _transactionService.CreateAsync(new Transaction()
            {
                Description = request.Description,
                IsMonthlyRecurrence = request.IsMonthlyRecurrence,
                IsPaid = request.IsPaid,
                UserId = ObjectId.Parse(AuthenticatedUserHelper.GetUserId(HttpContext)),
                TransactionType = request.TransactionType,
                Category = new TransactionCategory()
                {
                    Id = ObjectId.Parse(request.Category.Id),
                    Description = request.Category.Description,
                    Icon = request.Category.Icon
                },
                Amount = request.Amount,
                ExpenseDate = request.ExpenseDate
            });

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
            // TODO: colocar automapper.
            var result = await _transactionService.UpdateAsync(new Transaction()
            {
                Id = ObjectId.Parse(request.Id),
                Description = request.Description,
                IsMonthlyRecurrence = request.IsMonthlyRecurrence,
                IsPaid = request.IsPaid,
                UserId = ObjectId.Parse(AuthenticatedUserHelper.GetUserId(HttpContext)),
                Category = new TransactionCategory()
                {
                    Id = ObjectId.Parse(request.Category.Id),
                    Description = request.Category.Description,
                    Icon = request.Category.Icon
                },
                ExpenseDate = request.ExpenseDate
            });

            return ResponseHelper.Handle(result);
        }

        /// <summary>
        /// Deleta uma transação por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _transactionService.DeleteAsync(AuthenticatedUserHelper.GetUserId(HttpContext), id);
            return ResponseHelper.Handle(result);
        }
    }
}
