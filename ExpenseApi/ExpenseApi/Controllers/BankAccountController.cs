using Microsoft.AspNetCore.Mvc;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Helper;
using ExpenseApi.Service;
using Microsoft.AspNetCore.Authorization;
using ExpenseApi.Domain.Models.BankAccount;

namespace ExpenseApi.Controllers
{
    /// <summary>
    /// API para controlar as contas do banco.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService _bankService;

        public BankAccountController(IBankAccountService bankService)
        {
            _bankService = bankService;
        }

        /// <summary>
        /// Recupera todas as contas de um usuário
        /// </summary>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var results = await _bankService.GetAllAsync(AuthenticatedUserHelper.GetUserId(HttpContext));
            return ResponseHelper.Handle(results);
        }

        /// <summary>
        /// Recupera uma conta por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _bankService.GetByIdAsync(AuthenticatedUserHelper.GetUserId(HttpContext), id);
            return ResponseHelper.Handle(result);
        }

        /// <summary>
        /// Cria uma nova conta
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BankAccountRequestModel request)
        {
            // TODO: colocar automapper.
            var entity = new BankAccount()
            {
                Name = request.Name,
                IsMain = request.IsMain,
                Type = request.Type,
                UserId = AuthenticatedUserHelper.GetUserId(HttpContext)
            };
            entity.Deposit(request.AccountValue);

            var result = await _bankService.CreateAsync(entity);

            return ResponseHelper.Handle(result);
        }

        /// <summary>
        /// Altera uma conta
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] BankAccountRequestModel request)
        {
            // TODO: colocar automapper.
            var entity = new BankAccount()
            {
                Id = request.Id ?? Guid.Empty,
                Name = request.Name,
                IsMain = request.IsMain,
                Type = request.Type,
                UserId = AuthenticatedUserHelper.GetUserId(HttpContext)
            };

            var result = await _bankService.UpdateAsync(entity);

            return ResponseHelper.Handle(result);
        }

        /// <summary>
        /// Deleta uma conta por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _bankService.DeleteAsync(AuthenticatedUserHelper.GetUserId(HttpContext), id);
            return ResponseHelper.Handle(result);
        }

        /// <summary>
        /// Deposita um valor em uma conta
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpPut("deposit")]
        public async Task<IActionResult> Deposit([FromBody] BankAccountBalanceRequestModel request)
        {
            var result = await _bankService.DepositAsync(AuthenticatedUserHelper.GetUserId(HttpContext), request.Id, request.Amount);
            return ResponseHelper.Handle(result);
        }

        /// <summary>
        /// Debita um valor em uma conta
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpPut("withdraw")]
        public async Task<IActionResult> WithDraw([FromBody] BankAccountBalanceRequestModel request)
        {
            var result = await _bankService.WithDrawAsync(AuthenticatedUserHelper.GetUserId(HttpContext), request.Id, request.Amount);
            return ResponseHelper.Handle(result);
        }
    }
}
