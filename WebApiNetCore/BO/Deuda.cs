using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiNetCore.BO
{
    public class ListCubo
    {
        public List<CuboDeudaBO> data { get; set; }
    }
    public class CuboDeudaBO
    {
        public string Actividad { get; set; }
        public string Actualizacion { get; set; }
        public string Analista { get; set; }
        public string AprobadoPor { get; set; }
        public int Asiento { get; set; }
        public string Canal { get; set; }
        public string Chofer { get; set; }
        public string Cliente { get; set; }
        public string Cliente_Categoria { get; set; }
        public string Cliente_ID { get; set; }
        public string CodZona { get; set; }
        public string Comentario_OV { get; set; }
        public string CtaContable { get; set; }
        public string Dia_Ruta { get; set; }
        public int Dias { get; set; }
        public string DiasVencimiento { get; set; }
        public string Distrito { get; set; }
        public string DocumentoInterno { get; set; }
        public string DocumentoLegal { get; set; }
        public string Domicilio { get; set; }
        public string Domicilio_ID { get; set; }
        public string Estado_Despacho { get; set; }
        public string Estado_Sunat { get; set; }
        public string FIRST_IDDP { get; set; }
        public string FIRST_IDPR { get; set; }
        public string FechaCont { get; set; }
        public string FechaCreacionCliente { get; set; }
        public string FechaEmision { get; set; }
        public string FechaVencimiento { get; set; }
        //public string Fecha_Despacho { get; set; }
        //public string Fecha_UltPago { get; set; }
        public string Gerencia { get; set; }
        public string Giro_Negocio { get; set; }
        public string GrupoUnidadNegocio { get; set; }
        public string IDDIST { get; set; }
        public string Latitud { get; set; }
        public decimal Linea_Credito { get; set; }
        public string Longitud { get; set; }
        public decimal MontoDocumento { get; set; }
        public decimal Monto_UltPago { get; set; }
        public string Pais { get; set; }
        public string Periodo_ID { get; set; }
        public string Periodo_TipoGerenciaID { get; set; }
        public string Periodo_VendedorID { get; set; }
        public string Procedencia { get; set; }
        public string PuntoEmision { get; set; }
        public decimal Saldo { get; set; }
        public string Sectorista { get; set; }
        public string Supervisor { get; set; }
        public string Telefono_1 { get; set; }
        public string Telefono_2 { get; set; }
        public string TerminoPago { get; set; }
        public string TerminoPago_Resumen { get; set; }
        public string TipoDocumento { get; set; }
        public string TipoGerencia { get; set; }
        public string Ubigeo { get; set; }
        public string UnidadNegocio { get; set; }
        public string Usuario_Analista_ID { get; set; }
        public string Vendedor { get; set; }
        public string Vendedor_ID { get; set; }
        public string Zona { get; set; }
        public string Zona_ID { get; set; }
    }
}
