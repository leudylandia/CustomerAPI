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
                config.CreateMap<ClienteDto, Cliente>()
                .ForMember(x => x.Age, opt => opt.MapFrom(x => x.Edad)); //Add
                config.CreateMap<Cliente, ClienteDto>(); //Get
            });

            return mappingConfig;
        }
    }
}
