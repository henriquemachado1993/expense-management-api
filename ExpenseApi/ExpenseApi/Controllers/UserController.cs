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
            return ResponseHelper.Handle(await _userService.GetAllAsync());
        }

        /// <summary>
        /// Recupera um usuário por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return ResponseHelper.Handle(await _userService.GetByIdAsync(id));
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
            return ResponseHelper.Handle(await _userService.FindAsync(x => x.Name.Contains(name)));
        }

        /// <summary>
        /// Cria um novo usuário
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(UserRequestModel user)
        {
            // TODO: colocar automapper.
            var result = await _userService.CreateAsync(new User()
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

            return ResponseHelper.Handle(result);
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
            // TODO: colocar automapper.
            var result = await _userService.UpdateAsync(new User()
            {
                Id = user.Id ?? Guid.Empty,
                Name = user.Name,
                BirthDate = user.BirthDate,
                Password = user.Password,
                Address = new Address()
                {
                    City = user.Address.City,
                    Street = user.Address.Street,
                    ZipCode = user.Address.ZipCode
                }
            });

            return ResponseHelper.Handle(result);
        }

        /// <summary>
        /// Altera um senha somente se estiver logado
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpPut("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequestModel request)
        {
            var result = await _userService.UpdatePasswordAsync(AuthenticatedUserHelper.GetUserId(HttpContext), request.OldPassword, request.NewPassword);
            return ResponseHelper.Handle(result);
        }

        /// <summary>
        /// Deleta um usuário por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return ResponseHelper.Handle(await _userService.DeleteAsync(id));
        }
    }
}
