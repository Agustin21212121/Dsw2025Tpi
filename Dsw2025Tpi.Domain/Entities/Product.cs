using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities
{
    public class Product : EntityBase
    {
        public string sku {  get; set; }
        public string internalCode { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal currentUnitPrice { get; set; }
        public int stockQuantity{  get; set; }
        public bool isActive { get; set; }
    }
}
