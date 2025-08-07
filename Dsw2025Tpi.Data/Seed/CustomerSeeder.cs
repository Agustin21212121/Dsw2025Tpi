using Dsw2025Tpi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Data.Seed
{
    public static class CustomerSeeder
    {
        public static async Task SeedCustomersAsync(DbContext context)
        {
            if (!context.Set<Customer>().Any())
            {
                var jsonPath = Path.Combine(AppContext.BaseDirectory, "Sources", "customers.json");
                if (!File.Exists(jsonPath)) return;

                var jsonData = await File.ReadAllTextAsync(jsonPath);
                var customers = JsonSerializer.Deserialize<List<Customer>>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (customers != null && customers.Any())
                {
                    context.AddRange(customers);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}



