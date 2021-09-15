using CustomerAPI.Models.Dto;
using CustomerAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        protected ResponseDto _response;

        public UsersController(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
            _response = new ResponseDto();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserDto user)
        {
            var rpt = await _userRepository.Resgister(new Models.User
            {
                UserName = user.User.UserName
            }, user.Password);

            if (rpt == -1) //Existe
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "El usuario ya existe";
                return BadRequest(_response);
            }

            if (rpt == -500) //Error
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error creando usuario";
                return BadRequest(_response);
            }

            _response.DisplayMessage = "Usuario creado";
            _response.IsSuccess = true;
            _response.Result = rpt;

            return Ok(_response);
        }
    }
}
