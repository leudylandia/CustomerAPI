using CustomerAPI.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.Repository
{
    public interface IClienteRepository
    {
        Task<List<ClienteDto>> GetClientes();
        Task<ClienteDto> GetClienteById(int id);
        Task<ClienteDto> CreateUpdate(ClienteDto clienteDto);
        Task<bool> DeleteCliente(int id);
    }
}
