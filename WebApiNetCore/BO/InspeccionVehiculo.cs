using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiNetCore.BO
{
    public class InspeccionVehiculo
    {
        public string U_fecha_inspeccion { get; set; }
        public string U_hora_inspeccion { get; set; }
        public string U_conductor { get; set; }
        public string U_telefono { get; set; }
        public string U_placa { get; set; }
        public string U_marca { get; set; }
        public string U_modelo { get; set; }
        public string U_licencia { get; set; }
        public string U_tipo_vehiculo { get; set; }
        public double U_kilometraje_inicial { get; set; }
        public string U_fecha_emision_tarjeta_propiedad { get; set; }
        public string U_tarjeta_circulacion { get; set; }
        public string U_emision_soat { get; set; }
        public string U_fecha_emision_revision_tecnica { get; set; }
        public string U_observaciones { get; set; }
        public string U_puntos_mejora { get; set; }
        public List<VISDISHLI1Collection> VIS_DIS_HLI1Collection { get; set; }
    }

    public class VISDISHLI1Collection
    {
        public string U_seccion { get; set; }
        public string U_pregunta { get; set; }
        public string U_respuesta { get; set; }
    }
}
