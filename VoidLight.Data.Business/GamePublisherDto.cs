using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Data.Business
{
    public class GamePublisherDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public ICollection<GameDto> Games {get; set; }
    }
}
