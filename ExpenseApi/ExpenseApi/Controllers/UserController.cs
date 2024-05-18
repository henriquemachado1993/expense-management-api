using AutoMapper;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Domain.Models.User;
using ExpenseApi.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        private readonly IMapper _mapper;

        /// <summary>
        /// API para controlar usuário.
        /// </summary>
        public UserController(IUserService service, IMapper mapper)
        {
            _userService = service;
            _mapper = mapper;
    }

        /// <summary>
        /// Recupera todos os usuários
        /// </summary>
        /// <returns></returns>
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
            var result = await _userService.CreateAsync(_mapper.Map<User>(user));
            return ResponseHelper.Handle(result);
        }

        /// <summary>
        /// Altera um usuário
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UserRequestModel user)
        {
            var result = await _userService.UpdateAsync(_mapper.Map<User>(user));

            return ResponseHelper.Handle(result);
        }

        /// <summary>
        /// Altera um senha somente se estiver logado
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequestModel request)
        {
            var result = await _userService.UpdatePasswordAsync(AuthenticatedUserHelper.GetId(HttpContext), request.OldPassword, request.NewPassword);
            return ResponseHelper.Handle(result);
        }

        /// <summary>
        /// Deleta um usuário por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return ResponseHelper.Handle(await _userService.DeleteAsync(id));
        }
    }
}
