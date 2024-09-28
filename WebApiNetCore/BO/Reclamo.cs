using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace WebApiNetCore.BO
{
    public class ReclamoBo
    {
        public int DocEntry { get; set; }
        public string U_CardCode { get; set; }
        public string U_Address { get; set; }
        public string U_SlpSec { get; set; }
        public string U_SlpCode { get; set; }
        public string U_NumEN { get; set; }
        public string U_NumFT { get; set; }
        public List<ReclamoDetail> VIS_CMP_RQS1Collection { get; set; }
    }
    public class ReclamoDetail
    {
        public string U_ItemCode { get; set; }
        public string U_ItemName { get; set; }
        public string U_Reason { get; set; }
        public string U_Remark { get; set; }
        public decimal U_Quantity { get; set; }
        public string U_Batch { get; set; }

    }
    public class ReclamoResponse
    {
        public int DocEntry { get; set; }
        public string U_CardCode { get; set; }
        public string U_NumEN { get; set; }
        public string U_NumFT { get; set; }
    }





    /////////////////////////////////////////////////
    /////////////////////////////////////////////////
    /////////////////////////////////////////////////

    public class RootForm
    {
        public RootFormulario Formulario { get; set; }
    }

    public class RootFormulario
    {
        public string Name { get; set; }
        public dynamic Secciones { get; set; }
    }

    public class RootSeccion
    {
        public dynamic Preguntas { get; set; }
        public string Repetitivo { get; set; }
        public string SubTitulo { get; set; }
    }
    public class RootPregunta
    {
        public string Code { get; set; }
        public dynamic Respuesta { get; set; }
        public string Obligatorio { get; set; }
        public string TextoAyuda { get; set; }
        public string Pregunta { get; set; }
        public string Tipo { get; set; }
        public string TipoRespuesta { get; set; }
    }

    public class RootRespuesta
    {
        public string Code { get; set; }
        public int Orden { get; set; }
        public string Respuesta { get; set; }

    }


    //////////////////**********************/////////////////////////


    public class RootDataForm
    {
        public string CardCodeValue { get; set; }
        public string CardNameValue { get; set; }
        public string LicTradNum { get; set; }
        public dynamic Addresses { get; set; }
        public dynamic Invoices { get; set; }
    }





}
