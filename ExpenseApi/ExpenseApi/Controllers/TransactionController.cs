using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Models;
using Microsoft.AspNetCore.Authorization;
using ExpenseApi.Helper;

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
        private readonly string _userId;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
            _userId = AuthenticatedUserHelper.GetUserId(HttpContext);
        }

        /// <summary>
        /// Recupera todas as transações cadastradas
        /// </summary>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var results = await _transactionService.GetAllAsync(_userId);
            return Ok(results);
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
            var result = await _transactionService.GetByIdAsync(_userId, id);
            if (!result.IsValid)
                return BadRequest(result);
            if (result.Data == null)
                return NotFound(result);

            return Ok(result);
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
                UserId = ObjectId.Parse(request.UserId),
                TransactionType = request.TransactionType,
                Category = new TransactionCategory() { 
                    Id = ObjectId.Parse(request.Category.Id),
                    Description = request.Category.Description,
                    Icon = request.Category.Icon
                },
                Amount = request.Amount,
                ExpenseDate = request.ExpenseDate
            });

            if (!result.IsValid)
                BadRequest(result);
            
            return Ok(result);
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
                Description = request.Description,
                IsMonthlyRecurrence = request.IsMonthlyRecurrence,
                IsPaid = request.IsPaid,
                UserId = ObjectId.Parse(request.UserId),
                Category = new TransactionCategory()
                {
                    Id = ObjectId.Parse(request.Category.Id),
                    Description = request.Category.Description,
                    Icon = request.Category.Icon
                },
                ExpenseDate = request.ExpenseDate
            });

            if (!result.IsValid)
                BadRequest(result);

            return Ok(result);
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
            var result = await _transactionService.GetByIdAsync(_userId, id);

            if (!result.IsValid)
                return BadRequest(result);
            
            if (result.Data == null)
                return NotFound(result);
            
            await _transactionService.DeleteAsync(_userId, id);

            return NoContent();
        }
    }
}
