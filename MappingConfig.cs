using AutoMapper;
using CustomerAPI.Models;
using CustomerAPI.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI
{
    public class MappingConfig
    {
        //CLASE PARA MAPERAR NUESTRO MODELO CON EL DTO

        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ClienteDto, Cliente>();
                config.CreateMap<Cliente, ClienteDto>();
            });

            return mappingConfig;
        }
    }
}
