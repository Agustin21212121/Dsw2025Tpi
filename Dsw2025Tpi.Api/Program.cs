
using AspNetCoreRateLimit;
using Dsw2025Tpi.Data;
using Dsw2025Tpi.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;


namespace Dsw2025Tpi.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings.GetValue<string>("SecretKey");

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings["Issuer"],

                ValidateAudience = true,
                ValidAudience = jwtSettings["Audience"],

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),

                ClockSkew = TimeSpan.Zero
            };
        });


        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Bearer {token}",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
          new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
              Type = ReferenceType.SecurityScheme,
              Id = "Bearer"
            }
          },
          new string[] { }
        }
    });
        });

        builder.Services.AddHealthChecks();
        builder.Services.AddMemoryCache();
        builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
        builder.Services.AddInMemoryRateLimiting();
        builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        builder.Services.AddDbContext<Dsw2025TpiContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<Dsw2025TpiContext>();

            try
            {
                if (!db.Customers.Any())
                {
                    Console.WriteLine("📥 Cargando clientes desde customers.json...");
                    var json = File.ReadAllText("customers.json");
                    var customers = JsonSerializer.Deserialize<List<Customer>>(json);

                    if (customers != null && customers.Any())
                    {
                        db.Customers.AddRange(customers);
                        db.SaveChanges();
                        Console.WriteLine("✅ Clientes cargados correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("⚠️ No se encontraron clientes en el archivo JSON.");
                    }
                }
                else
                {
                    Console.WriteLine("ℹ️ Ya existen clientes en la base. No se cargan.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al cargar clientes: {ex.Message}");
            }
        }

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<Dsw2025TpiContext>();

            // 👤 Crear usuario admin si no existe
            if (!db.Users.Any(u => u.Username == "admin"))
            {
                Console.WriteLine("🛠 Creando usuario admin...");

                string password = "admin123";
                string hashedPassword = HashPassword(password);

                var admin = new User
                {
                    Username = "admin",
                    PasswordHash = hashedPassword,
                    Role = "Admin"
                };

                db.Users.Add(admin);
                db.SaveChanges();

                Console.WriteLine("✅ Usuario admin creado.");
            }
            else
            {
                Console.WriteLine("ℹ️ Ya existe el usuario admin.");
            }
        }

        // 🔒 Función para hashear con SHA256
        string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes);
            }
        }



        //app.UseHttpsRedirection();


        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.MapHealthChecks("/healthcheck");

        app.UseIpRateLimiting();

        app.Run();

        
        }

    }

