using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VoidLight.Data.Business;
using VoidLight.Data.Entities;

namespace VoidLight.Business.Services.Contracts
{
    public interface IExampleService
    {
        public void ExampleMethod();
        public IAsyncEnumerable<ExampleDto> GetAll();
        public Task<Example> AddExample(ExampleDto dto);
        public Task DeleteExample(int id);
    }
}
