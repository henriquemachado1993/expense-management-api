using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Models;

namespace ExpenseApi.Controllers
{
    /// <summary>
    /// API para controlar despesas.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        /// <summary>
        /// Recupera todas as depesas cadastradas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var results = await _expenseService.GetAllAsync();
            return Ok(results);
        }

        /// <summary>
        /// Recupera uma despesa por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _expenseService.GetByIdAsync(id);
            if (!result.IsValid)
                return BadRequest(result);
            if (result.Data == null)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// Cadatra uma nova despesa
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ExpenseRequestModel request)
        {
            // TODO: colocar automapper.
            var result = await _expenseService.CreateAsync(new Expense()
            {
                Description = request.Description,
                IsMonthlyRecurrence = request.IsMonthlyRecurrence,
                IsPaid = request.IsPaid,
                UserId = ObjectId.Parse(request.UserId),
                Category = new ExpenseCategory() { 
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
        /// Alterar uma despesa
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ExpenseRequestModel request)
        {
            var expenseResult = await _expenseService.GetByIdAsync(request.Id);
            if (!expenseResult.IsValid)
                return BadRequest(expenseResult);
            
            if (expenseResult.Data == null)
                return NotFound(expenseResult);

            // TODO: colocar automapper.
            var result = await _expenseService.UpdateAsync(new Expense()
            {
                Description = request.Description,
                IsMonthlyRecurrence = request.IsMonthlyRecurrence,
                IsPaid = request.IsPaid,
                UserId = ObjectId.Parse(request.UserId),
                Category = new ExpenseCategory()
                {
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
        /// Deleta uma despesa por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _expenseService.GetByIdAsync(id);

            if (!result.IsValid)
                return BadRequest(result);
            
            if (result.Data == null)
                return NotFound(result);
            
            await _expenseService.DeleteAsync(id);

            return NoContent();
        }
    }
}
