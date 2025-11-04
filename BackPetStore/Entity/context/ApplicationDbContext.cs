using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

namespace Entity.Context
{
    /// <summary>
    /// Contexto principal de la aplicación Tienda de Mascotas.
    /// Integra Entity Framework Core y Dapper para soporte híbrido de consultas ORM y SQL directo.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        #region === CONFIGURACIONES EF CORE ===

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define esquema por defecto
            modelBuilder.HasDefaultSchema("Tienda");

            // Precisión global para decimales
            modelBuilder.Entity<VentaProducto>().Property(vp => vp.PrecioUnitario).HasPrecision(18, 2);
            modelBuilder.Entity<Pago>().Property(p => p.Monto).HasPrecision(18, 2);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Permite logs detallados en entorno de desarrollo
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
        }

        #endregion

        #region === OVERRIDES SAVECHANGES (AUDITORÍA LIGERA) ===

        public override int SaveChanges()
        {
            EnsureAudit();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            EnsureAudit();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void EnsureAudit()
        {
            ChangeTracker.DetectChanges();
            // Aquí podrías agregar campos de auditoría si tu modelo los tuviera (CreatedDate, UpdatedDate, etc.)
        }

        #endregion

        #region === DAPPER INTEGRATION ===

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null, int? timeout = null, CommandType? type = null)
        {
            using var command = new DapperEFCoreCommand(this, sql, parameters, timeout, type, CancellationToken.None);
            var connection = Database.GetDbConnection();
            return await connection.QueryAsync<T>(command.Definition);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null, int? timeout = null, CommandType? type = null)
        {
            using var command = new DapperEFCoreCommand(this, sql, parameters, timeout, type, CancellationToken.None);
            var connection = Database.GetDbConnection();
            return await connection.QueryFirstOrDefaultAsync<T>(command.Definition);
        }

        public async Task<int> ExecuteAsync(string sql, object parameters = null, int? timeout = null, CommandType? type = null)
        {
            using var command = new DapperEFCoreCommand(this, sql, parameters, timeout, type, CancellationToken.None);
            var connection = Database.GetDbConnection();
            return await connection.ExecuteAsync(command.Definition);
        }

        public async Task<T> ExecuteScalarAsync<T>(string sql, object parameters = null, int? timeout = null, CommandType? type = null)
        {
            using var command = new DapperEFCoreCommand(this, sql, parameters, timeout, type, CancellationToken.None);
            var connection = Database.GetDbConnection();
            return await connection.ExecuteScalarAsync<T>(command.Definition);
        }

        public readonly struct DapperEFCoreCommand : IDisposable
        {
            public CommandDefinition Definition { get; }

            public DapperEFCoreCommand(DbContext context, string sql, object parameters, int? timeout, CommandType? type, CancellationToken ct)
            {
                var transaction = context.Database.CurrentTransaction?.GetDbTransaction();
                var commandType = type ?? CommandType.Text;
                var commandTimeout = timeout ?? context.Database.GetCommandTimeout() ?? 30;

                Definition = new CommandDefinition(
                    sql, parameters, transaction, commandTimeout, commandType, cancellationToken: ct
                );
            }

            public void Dispose() { }
        }

        #endregion

        #region === DBSETS (TABLAS) ===

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Mascota> Mascotas { get; set; }
        public DbSet<MascotaCliente> MascotaClientes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<VentaProducto> VentaProductos { get; set; }
        public DbSet<Pago> Pagos { get; set; }

        #endregion
    }
}
