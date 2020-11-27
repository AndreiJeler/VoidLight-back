using System;
using System.Collections.Generic;
using System.Text;
using VoidLight.Data.Business;

namespace VoidLight.Business.Services.Contracts
{
    public interface IGamePublisherService
    {
        public IAsyncEnumerable<GamePublisherDto> GetGamePublisher();
    }
}
