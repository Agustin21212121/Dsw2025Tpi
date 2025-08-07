using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities
{
    public class Product : EntityBase
    {

        public Product() { }
        public Product(string sku, string name, decimal currentUnitPrice, int stockQuantity)
        {
            Sku = sku;
            Name = name;
            CurrentUnitPrice = currentUnitPrice;
            StockQuantity = stockQuantity;
            IsActive = true;
        }

        public string Sku { get; set; } = default!;

        public string InternalCode { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        public decimal CurrentUnitPrice { get; set; }

        public int StockQuantity { get; set; }

        public bool IsActive { get; set; } = true;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    }
}
