using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Models;

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
        public async Task<ActionResult<List<User>>> Get()
        {
            var result = await _userService.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Recupera um usuário por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(string id)
        {
            var result = await _userService.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Recupera um usuário pelo nome
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpGet("get-by-name/{name}")]
        public async Task<ActionResult<List<User>>> GetByName(string name)
        {
            var result = await _userService.FindAsync(x => x.Name.Contains(name));

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Cria um novo usuário
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UserRequestModel user)
        {
            // TODO: colocar automapper.
            await _userService.CreateAsync(new User()
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
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        /// <summary>
        /// Altera um usuário
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] UserRequestModel user)
        {
            if (await _userService.GetByIdAsync(user.Id) == null)
            {
                return NotFound();
            }

            // TODO: colocar automapper.
            return Ok(await _userService.UpdateAsync(new User()
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
            }));
        }

        /// <summary>
        /// Deleta um usuário por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var produto = await _userService.GetByIdAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            await _userService.DeleteAsync(id);

            return NoContent();
        }
    }
}
