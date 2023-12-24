using Microsoft.AspNetCore.Mvc;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Helper;
using ExpenseApi.Service;
using Microsoft.AspNetCore.Authorization;
using ExpenseApi.Domain.Models.Transaction;
using AutoMapper;

namespace ExpenseApi.Controllers
{
    /// <summary>
    /// API para controlar categorias das transações.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class TransactionCategoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITransactionCategoryService _categoryService;

        public TransactionCategoryController(IMapper mapper, ITransactionCategoryService categoryService)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        /// <summary>
        /// Recupera todas as categoria
        /// </summary>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var results = await _categoryService.GetAllAsync();
            return ResponseHelper.Handle(results);
        }

        /// <summary>
        /// Recupera uma categoria por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            return ResponseHelper.Handle(result);
        }

        /// <summary>
        /// Cadatra uma nova categoria
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TransactionCategoryRequestModel request)
        {
            var result = await _categoryService.CreateAsync(_mapper.Map<TransactionCategory>(request));

            return ResponseHelper.Handle(result);
        }

        /// <summary>
        /// Alterar uma categoria
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] TransactionCategoryRequestModel request)
        {
            var result = await _categoryService.UpdateAsync(_mapper.Map<TransactionCategory>(request));
            return ResponseHelper.Handle(result);
        }

        /// <summary>
        /// Deleta uma categoria por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _categoryService.DeleteAsync(id);
            return ResponseHelper.Handle(result);
        }
    }
}
