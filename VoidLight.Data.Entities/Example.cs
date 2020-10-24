using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Data.Entities
{
    public class Example
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Value { get; set; }


    }
    public class BoardConfiguration : IEntityTypeConfiguration<Example>
    {
        public void Configure(EntityTypeBuilder<Example> builder)
        {
            builder
                .HasKey(ex => ex.Id);
           
        }
    }
}
