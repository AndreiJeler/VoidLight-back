using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VoidLight.Business.Services.Contracts;
using VoidLight.Data;
using VoidLight.Data.Business;

namespace VoidLight.Business.Services
{
    public class GamePublisherService : IGamePublisherService
    {
        private readonly VoidLightDbContext _context;

        public GamePublisherService(VoidLightDbContext context)
        {
            _context = context;
        }
        public IAsyncEnumerable<GamePublisherDto> GetGamePublisher()
        {
            return _context.GamePublishers.Select(gp => new GamePublisherDto()
            {
                Id = gp.Id,
                Name = gp.Name,
                Url = gp.Url,
                /*Games = gp.Games.Select(g => new GameDto()
                {
                    Description = g.Description,
                    Id = g.Id,
                    Name = g.Name,
                    Publisher = gp.Name,
                    IsFavourite = false
                }).ToList()*/
            }).AsAsyncEnumerable();
        }
    }
}
