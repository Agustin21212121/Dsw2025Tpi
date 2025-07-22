using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities
{
    public class Customer : EntityBase
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }

        [JsonIgnore]//ver para que es.
        public ICollection<Order> Orders { get; set; }

    }
}
