using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomerAPI.Data;
using CustomerAPI.Models;
using CustomerAPI.Repository;
using CustomerAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;

namespace CustomerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteRepository _clienteRepository;
        protected ResponseDto _response;

        public ClientesController(IClienteRepository clienteRepository)
        {
            this._clienteRepository = clienteRepository;
            _response = new ResponseDto();
        }

        // GET: api/Clientes
        [HttpGet]
        public async Task<ActionResult> GetClientes()
        {
            try
            {
                var lista = await _clienteRepository.GetClientes();
                _response.Result = lista;
                _response.DisplayMessage = "Lista de clientes";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return Ok(_response);
        }

        [HttpGet]
        [Route("/GetClientesResponse")]
        public async Task<ActionResult<IEnumerable<ResponseDto>>> GetClientesResponse()
        {
            try
            {
                var lista = await _clienteRepository.GetClientes();
                _response.Result = lista;
                _response.DisplayMessage = "Lista de clientes";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return Ok(_response);
        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _clienteRepository.GetClienteById(id);

            if (cliente == null)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Cliente no existe";
                return NotFound(_response);
            }

            _response.Result = cliente;
            _response.DisplayMessage = "Informacion del cliente";

            return Ok(_response);
        }

        // PUT: api/Clientes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, ClienteDto clienteDto)
        {
            try
            {
                if (clienteDto == null)
                {
                    _response.IsSuccess = false;
                    _response.DisplayMessage = "Ha ocurrido un inconveniente tratando de agregar el registro";
                    return BadRequest(_response);
                }

                var model = await _clienteRepository.CreateUpdate(clienteDto);

                _response.Result = model;
                _response.DisplayMessage = "Se ha completado correctamente";
                
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.ErrorMessages = new List<string> { ex.ToString() };
                _response.DisplayMessage = "Ha ocurrido un inconveniente tratando de agregar el registro";
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
        }

        // POST: api/Clientes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(ClienteDto clienteDto)
        {
            try
            {
                if (clienteDto == null)
                {
                    _response.IsSuccess = false;
                    _response.DisplayMessage = "Ha ocurrido un inconveniente tratando de agregar el registro";
                    return BadRequest(_response);
                }

                var model = await _clienteRepository.CreateUpdate(clienteDto);
                _response.Result = model;
                _response.DisplayMessage = "Se ha completado el registro";

                return CreatedAtAction("GetCliente", new { id = model.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.DisplayMessage = "Ocurrió un incoveniente intentado de guardar el registro";
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
                return BadRequest(_response);
            }
        }

        // DELETE: api/Clientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            try
            {
                if (id < 1)
                {
                    _response.IsSuccess = false;
                    _response.DisplayMessage = "Ha ocurrido un inconveniente.";
                    return BadRequest(_response);
                }

                var result = await _clienteRepository.DeleteCliente(id);

                if (result)
                {
                    _response.IsSuccess = result;
                    _response.DisplayMessage = "Registro eliminado";
                }
                else
                {
                    _response.IsSuccess = result;
                    _response.DisplayMessage = "Registro no eliminado";
                }

                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.DisplayMessage = "Ocurrió un incoveniente intentado eliminar el registro";
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
                return BadRequest(_response);
            }
        }
    }
}
