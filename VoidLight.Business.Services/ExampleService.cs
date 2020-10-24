using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidLight.Business.Services.Contracts;
using VoidLight.Data;
using VoidLight.Data.Business;
using VoidLight.Data.Entities;
using VoidLight.Data.Mappers;

namespace VoidLight.Business.Services
{
    public class ExampleService : IExampleService
    {
        private readonly VoidLightDbContext _dbContext;
        public ExampleService(VoidLightDbContext context)
        {
            _dbContext = context;
        }


        public async Task<Example> AddExample(ExampleDto dto)
        {
            var example = ExampleMapper.ConvertDtoToEntity(dto);
            await _dbContext.AddAsync(example);
            await _dbContext.SaveChangesAsync();
            return example;
        }

        public void ExampleMethod()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<ExampleDto> GetAll()
        {
            return this._dbContext.Examples.Select(example => ExampleMapper.ConvertEntityToDto(example)).AsAsyncEnumerable();
        }
    }
}
