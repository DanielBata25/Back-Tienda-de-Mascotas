using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Data.Init
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Ejecuta migraciones pendientes
            context.Database.Migrate();

            // ============================================
            //  CLIENTES
            // ============================================
            if (!context.Clientes.Any())
            {
                var cliente1 = new Cliente
                {
                    Nombre = "Roberto",
                    Apellido = "Toby",
                    Email = "camilo.charry@correo.com",
                    Telefono = "3001234567",
                    Direccion = "Neiva - Huila",
                    Password = HashPassword("12345"),
                    IsDeleted = false
                };

                var cliente2 = new Cliente
                {
                    Nombre = "Laura",
                    Apellido = "Michel",
                    Email = "laura.michel@correo.com",
                    Telefono = "3109876543",
                    Direccion = "Pitalito - Huila",
                    Password = HashPassword("12345"),
                    IsDeleted = false
                };

                context.Clientes.AddRange(cliente1, cliente2);
                context.SaveChanges();
            }

            // ============================================
            //  MASCOTAS
            // ============================================
            if (!context.Mascotas.Any())
            {
                var mascota1 = new Mascota
                {
                    Nombre = "Luna",
                    Especie = "Perro",
                    Raza = "Labrador",
                    FechaNacimiento = new DateTime(2020, 3, 15),
                    Genero = "Hembra",
                    Notas = "Amigable y activa",
                    IsDeleted = false
                };

                var mascota2 = new Mascota
                {
                    Nombre = "Simba",
                    Especie = "Gato",
                    Raza = "Angora",
                    FechaNacimiento = new DateTime(2021, 11, 2),
                    Genero = "Macho",
                    Notas = "Curioso y dormilón",
                    IsDeleted = false
                };

                context.Mascotas.AddRange(mascota1, mascota2);
                context.SaveChanges();
            }


            if (!context.MascotaClientes.Any())
            {
                var mascotas = context.Mascotas.OrderBy(m => m.Id).ToList();
                var clientes = context.Clientes.OrderBy(c => c.Id).ToList();

                var mascotaCliente1 = new MascotaCliente
                {
                    MascotaId = mascotas.First().Id,
                    MascotaNombre = mascotas.First().Nombre, // 👈 agregado
                    ClienteId = clientes.First().Id,
                    ClienteNombre = clientes.First().Nombre + " " + clientes.First().Apellido, // 👈 agregado
                    FechaRegistro = DateTime.Now,
                    IsDeleted = false
                };

                var mascotaCliente2 = new MascotaCliente
                {
                    MascotaId = mascotas.Last().Id,
                    MascotaNombre = mascotas.Last().Nombre, // 👈 agregado
                    ClienteId = clientes.Last().Id,
                    ClienteNombre = clientes.Last().Nombre + " " + clientes.Last().Apellido, // 👈 agregado
                    FechaRegistro = DateTime.Now,
                    IsDeleted = false
                };

                context.MascotaClientes.AddRange(mascotaCliente1, mascotaCliente2);
                context.SaveChanges();
            }

            // ============================================
            //  PRODUCTOS
            // ============================================
            if (!context.Productos.Any())
            {
                var producto1 = new Producto
                {
                    Nombre = "Concentrado DogChow",
                    Categoria = "Alimento",
                    Descripcion = "Bolsa de 10kg para perro adulto",
                    Stock = 25,
                    Estado = "Disponible",
                    IsDeleted = false
                };

                var producto2 = new Producto
                {
                    Nombre = "Arena para gatos GatoClean",
                    Categoria = "Higiene",
                    Descripcion = "Bolsa de 5kg, neutraliza olores",
                    Stock = 50,
                    Estado = "Disponible",
                    IsDeleted = false
                };

                context.Productos.AddRange(producto1, producto2);
                context.SaveChanges();
            }

            // ============================================
            //  VENTAS
            // ============================================
            if (!context.Ventas.Any())
            {
                var venta1 = new Venta
                {
                    ClienteId = context.Clientes.OrderBy(c => c.Id).First().Id,
                    FechaVenta = DateTime.Now,
                    Estado = "Completada",
                    Canal = "Tienda Física",
                    Observaciones = "Pago en efectivo",
                    IsDeleted = false
                };

                context.Ventas.Add(venta1);
                context.SaveChanges();
            }

            // ============================================
            //  VENTA–PRODUCTO
            // ============================================
            if (!context.VentaProductos.Any())
            {
                var venta = context.Ventas.OrderBy(v => v.Id).First();
                var producto = context.Productos.OrderBy(p => p.Id).First();

                var ventaProducto = new VentaProducto
                {
                    VentaId = venta.Id,
                    ProductoId = producto.Id,
                    Cantidad = 2,
                    PrecioUnitario = 85000m,
                    IsDeleted = false
                };

                context.VentaProductos.Add(ventaProducto);
                context.SaveChanges();
            }

            // ============================================
            //  PAGOS
            // ============================================
            if (!context.Pagos.Any())
            {
                var pago1 = new Pago
                {
                    VentaId = context.Ventas.OrderBy(v => v.Id).First().Id,
                    FechaPago = DateTime.Now,
                    Monto = 170000m,
                    MetodoPago = "Efectivo",
                    EstadoPago = "Aprobado",
                    Referencia = "PAG-" + Guid.NewGuid().ToString("N")[..8],
                    IsDeleted = false
                };

                context.Pagos.Add(pago1);
                context.SaveChanges();
            }
        }

        // ============================================
        //  FUNCIONALIDAD: HASH DE CONTRASEÑAS
        // ============================================
        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
