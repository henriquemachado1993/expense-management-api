﻿using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Models;
using ExpenseApi.Helper;
using ExpenseApi.Service;
using Microsoft.AspNetCore.Authorization;

namespace ExpenseApi.Controllers
{
    /// <summary>
    /// API para controlar categorias.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ITransactionCategoryService _categoryService;

        public CategoryController(ITransactionCategoryService categoryService)
        {
            _categoryService = categoryService;
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
        public async Task<IActionResult> Get(string id)
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
            // TODO: colocar automapper.
            var result = await _categoryService.CreateAsync(new TransactionCategory()
            {
                Name = request.Name,
                Description = request.Description,
                Icon = request.Icon
            });

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
            // TODO: colocar automapper.
            var result = await _categoryService.UpdateAsync(new TransactionCategory()
            {
                Id = ObjectId.Parse(request.Id),
                Name = request.Name,
                Description = request.Description,
                Icon = request.Icon
            });
            return ResponseHelper.Handle(result);
        }

        /// <summary>
        /// Deleta uma categoria por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _categoryService.DeleteAsync(id);
            return ResponseHelper.Handle(result);
        }
    }
}
