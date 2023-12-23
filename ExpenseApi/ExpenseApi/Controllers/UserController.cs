using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Models;
using ExpenseApi.Domain.ValueObjects;
using ExpenseApi.Helper;

namespace ExpenseApi.Controllers
{
    /// <summary>
    /// API para controlar usuário.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService service)
        {
            _userService = service;
        }

        /// <summary>
        /// Recupera todos os usuários
        /// </summary>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _userService.GetAllAsync();

            if(!result.IsValid)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Recupera um usuário por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _userService.GetByIdAsync(id);
            if (!result.IsValid)
                return BadRequest(result);
            if (result.Data == null)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// Recupera um usuário pelo nome
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpGet("get-by-name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _userService.FindAsync(x => x.Name.Contains(name));
            if (!result.IsValid)
                return BadRequest(result);
            if (result.Data == null)
                return NotFound();
            
            return Ok(result);
        }

        /// <summary>
        /// Cria um novo usuário
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserRequestModel user)
        {
            // TODO: colocar automapper.
            var result  = await _userService.CreateAsync(new User()
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                BirthDate = user.BirthDate,
                Address = new Address()
                {
                    City = user.Address.City,
                    Street = user.Address.Street,
                    ZipCode = user.Address.ZipCode
                }
            });

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Altera um usuário
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UserRequestModel user)
        {
            var userResult = await _userService.GetByIdAsync(user.Id);
            if (!userResult.IsValid)
                return BadRequest(userResult);
            if (userResult == null)
                return NotFound(userResult);

            // TODO: colocar automapper.
            var result = await _userService.UpdateAsync(new User()
            {
                Id = ObjectId.Parse(user.Id),
                Name = user.Name,
                Email = user.Email,
                BirthDate = user.BirthDate,
                Password = user.Password,
                Address = new Address()
                {
                    City = user.Address.City,
                    Street = user.Address.Street,
                    ZipCode = user.Address.ZipCode
                }
            });

            if (!result.IsValid)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Deleta um usuário por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _userService.GetByIdAsync(id);

            if(!result.IsValid)
                return BadRequest(result);

            await _userService.DeleteAsync(id);

            return NoContent();
        }
    }
}
