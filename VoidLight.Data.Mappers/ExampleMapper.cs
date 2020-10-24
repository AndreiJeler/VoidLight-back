using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VoidLight.Data.Business;
using VoidLight.Data.Entities;

namespace VoidLight.Data.Mappers
{
    public class ExampleMapper
    {
        private static readonly MapperConfiguration _config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Example, ExampleDto>();
            cfg.CreateMap<ExampleDto, Example>();
        });

        private static readonly Mapper _mapper = new Mapper(_config);

        public static ExampleDto ConvertEntityToDto(Example example)
        {
            return _mapper.Map<ExampleDto>(example);
        }

        public static Example ConvertDtoToEntity(ExampleDto example)
        {
            return _mapper.Map<Example>(example);
        }
    }
}
