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
        public async Task<ActionResult<List<Expense>>> Get()
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
        public async Task<ActionResult<Expense>> Get(string id)
        {
            var result = await _expenseService.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Cadatra uma nova despesa
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ExpenseRequestModel request)
        {
            // TODO: colocar automapper.
            await _expenseService.CreateAsync(new Expense()
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

            return CreatedAtAction(nameof(Get), new { id = request.Id }, request);
        }

        /// <summary>
        /// Alterar uma despesa
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] ExpenseRequestModel request)
        {
            if (await _expenseService.GetByIdAsync(request.Id) == null)
            {
                return NotFound();
            }

            // TODO: colocar automapper.
            return Ok(await _expenseService.UpdateAsync(new Expense()
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
            }));
        }

        /// <summary>
        /// Deleta uma despesa por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var result = await _expenseService.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            await _expenseService.DeleteAsync(id);

            return NoContent();
        }
    }
}
