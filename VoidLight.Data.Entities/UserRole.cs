using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VoidLight.Data.Entities
{
    public enum RoleType
    {
        Admin = 1,
        Regular = 2,
        Streamer = 3,
        General = 4
    }
    public class UserRole
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
