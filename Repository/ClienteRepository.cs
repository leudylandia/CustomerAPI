using AutoMapper;
using CustomerAPI.Data;
using CustomerAPI.Models;
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

        public async Task<ClienteDto> CreateUpdate(ClienteDto clienteDto)
        {
            var cliente = new Cliente();
            if (clienteDto.Id == 0)
            {
                cliente = _mapper.Map<Cliente>(clienteDto);
                await _dbContext.AddAsync(cliente);
            }
            else
            {
                //var clienteDb = GetClienteById(clienteDto.Id);
                var cliente2 = _mapper.Map<Cliente>(clienteDto);

                cliente = _mapper.Map<ClienteDto, Cliente>(clienteDto);
                _dbContext.Update(cliente);
            }

            await _dbContext.SaveChangesAsync();

            var result = _mapper.Map<ClienteDto>(cliente);
            return result;
        }

        public async Task<bool> DeleteCliente(int id)
        {
            try
            {
                var cliente = await _dbContext.Clientes.FindAsync(id);

                if (cliente == null)
                    return false;

                _dbContext.Clientes.Remove(cliente);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception)
            {
                return false;
            }

            return true;
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
