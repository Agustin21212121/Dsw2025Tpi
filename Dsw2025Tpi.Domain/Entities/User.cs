using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities
{
    public class User : EntityBase
    {
      public string Username { get; set; }
      public string PasswordHash { get; set; }
      public string Role { get; set; } // "Admin", "Operador", etc.
    }
}
