using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Data.Entities
{
    public abstract class Content
    {
        public int Id { get; set; }
        public string ContentPath { get; set; }
    }
}
