using Entity.DTOs;
using Entity.Model;
using Mapster;

namespace Utilities.Mapping
{
    /// <summary>
    /// Configuración centralizada de mapeos entre entidades y DTOs usando Mapster.
    /// Esta clase debe ejecutarse en el arranque de la aplicación (Program.cs).
    /// </summary>
    public static class MapsterConfig
    {
        public static void RegisterMappings()
        {
     
            TypeAdapterConfig<Cliente, ClienteDto>.NewConfig();
            TypeAdapterConfig<ClienteDto, Cliente>.NewConfig();

  
            TypeAdapterConfig<Mascota, MascotaDto>.NewConfig();
            TypeAdapterConfig<MascotaDto, Mascota>.NewConfig();

      
            TypeAdapterConfig<Producto, ProductoDto>.NewConfig();
            TypeAdapterConfig<ProductoDto, Producto>.NewConfig();

           
            TypeAdapterConfig<Venta, VentaDto>.NewConfig();
            TypeAdapterConfig<VentaDto, Venta>.NewConfig();


            TypeAdapterConfig<Pago, PagoDto>.NewConfig();
            TypeAdapterConfig<PagoDto, Pago>.NewConfig();

            TypeAdapterConfig<MascotaCliente, MascotaClienteDto>.NewConfig();
            TypeAdapterConfig<MascotaClienteDto, MascotaCliente>.NewConfig();

   
            TypeAdapterConfig<VentaProducto, VentaProductoDto>.NewConfig();
            TypeAdapterConfig<VentaProductoDto, VentaProducto>.NewConfig();
        }
    }
}
