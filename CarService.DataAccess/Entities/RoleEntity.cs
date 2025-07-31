using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.DataAccess.Entities
{
    public class RoleEntity
    {
        public Guid Id { get; set; }       
        public string Name { get; set; }
        public ICollection<UserEntity> Users { get; set; } = [];
    }
}
