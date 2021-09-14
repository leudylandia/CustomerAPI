using AutoMapper;
using CustomerAPI.Data;
using CustomerAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public ClienteRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public Task<ClienteDto> CreateUpdate(ClienteDto clienteDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCliente(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ClienteDto> GetClienteById(int id)
        {
            var client = await _dbContext.Clientes.FirstOrDefaultAsync(c => c.Id == id);

            var result = _mapper.Map<ClienteDto>(client);

            return result;
        }

        public async Task<List<ClienteDto>> GetClientes()
        {
            var lista = await _dbContext.Clientes.ToListAsync();

            var result = _mapper.Map<List<ClienteDto>>(lista);

            return result;
            
        }
    }
}
