using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiNetCore.BO
{

    public class ResponseSurveyBO
    {
        public string DocEntry { get; set; }
        public string DocNum { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; }
    }

    public class SurveyBO
    {
        public string cardCode { get; set; }
        public string cardName { get; set; }
        public string chkrecibido { get; set; }
        public string DateCompletion { get; set; }
        public string DateCreation { get; set; }
        public int id { get; set; }
        public string IdDocument { get; set; }
        public string Imei { get; set; }
        public List<Section> Section { get; set; }
        public string shipToCode { get; set; }
        public int SlpCode { get; set; }
    }
    public class Section
    {
        public string U_Descr { get; set; }
        public int U_File { get; set; }
        public int id { get; set; }
        public string IdDocument { get; set; }
        public string U_Order { get; set; }
        public List<SubSeccion> SubSeccion { get; set; }
    }
    public class SubSeccion
    {
        public List<Formulario> Formulario { get; set; }
        public int DocEntry { get; set; }
        public string IdDocument { get; set; }
        public string Titulo_SubSeccion { get; set; }
        public string SubTitulo_SubSeccion { get; set; }
    }
    public class Formulario
    {
        public string Base64 { get; set; }
        public int U_File { get; set; }
        public int DocEntry { get; set; }
        public int id { get; set; }
        public string IdDocument { get; set; }
        public string U_Order { get; set; }
        public List<Pregunta> Preguntas { get; set; }
        public string U_Title { get; set; }
        public string Url { get; set; }
    }
    public class Pregunta
    {
        public List<Respuesta> Respuestas { get; set; }
        public string Base64 { get; set; }
        public string U_Descr { get; set; }
        public int id { get; set; }
        public string IdDocument { get; set; }
        public string U_Order { get; set; }
        public int U_Question { get; set; }
        public string U_Responsevalue { get; set; }
        public string U_Type { get; set; }
        public string Url { get; set; }
    }
    public class Respuesta
    {
        public string U_answer { get; set; }
        public int id { get; set; }
        public string IdDocument { get; set; }
        public string U_Order { get; set; }
        public string U_Responsevalue { get; set; }
    }

    public class VIS_MKT_OENC
    {
        public string U_CardCode { get; set; }
        public int U_SlpCode { get; set; }
        public List<VISMKTENC1Collection> VIS_MKT_ENC1Collection { get; set; }
    }
    public class VISMKTENC1Collection
    {
        public int U_SubSeccion { get; set; }
        public string U_Desc_SubSeccion { get; set; }
        public int U_Seccion { get; set; }
        public string U_Desc_Seccion { get; set; }
        public int U_Formulario { get; set; }
        public string U_Desc_Formulario { get; set; }
        public int U_Preguntas { get; set; }
        public string U_Desc_Pregunta { get; set; }
        public string U_Respuesta { get; set; }
        public object U_Foto { get; set; }
        public object U_FotoPregunta { get; set; }
        public object U_RespSelect { get; set; }
        public object U_RespText { get; set; }
    }

    public class ListEncuesta
    {
        public string DocNum { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string SlpCode { get; set; }
        public string SlpName { get; set; }
        public string CreateDate { get; set; }
    }
}
