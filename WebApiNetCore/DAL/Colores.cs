
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sap.Data.Hana;
using SAP_Core.BO;
using SAP_Core.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SAP_Core.DAL
{
    public class ColoresDAL : Connection, IDisposable
    {

        public ListaColores GetColores (string imei)
        {
            HanaDataReader reader;
            HanaConnection connection= GetConnection();
            ListaColores colores = new ListaColores();
            List<ColorHeaderBO> listColores = new List<ColorHeaderBO>();
            ColorHeaderBO color = new ColorHeaderBO();
            string strSQL = string.Format("CALL {0}.APP_COLORS ('{1}')", DataSource.bd(), imei);

            try{

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

                connection.Open();
            HanaCommand command = new HanaCommand(strSQL, connection);

            reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    color = new ColorHeaderBO();
                    color.Code = reader["Code"].ToString();
                    color.Description = reader["Dscription"].ToString();
                    color.Status = reader["Status"].ToString();
                    color.Detail = reader["Detail"] == null ? null : JsonConvert.DeserializeObject<List<ColorsDetailBO>>(reader["Detail"].ToString());
                    listColores.Add(color);
                }
            }
            colores.Colors = listColores;
            connection.Close();
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_GetColores - " + ex.Message + " "+ imei);
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                connection.Close();
            }
            finally
            {
                if (connection.State==ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return colores;
        }


        public ListColorRange GetlegendColor(string imei)
        {
            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            ColorRange colores = new ColorRange();
            ListColorRange listColorRange = new ListColorRange();

            List<ColorRange> Lcolores = new List<ColorRange>();
            string strSQL = string.Format("CALL {0}.APP_APPROVAL_COLOR ('{1}')", DataSource.bd(), imei);

            try
            {

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        colores = new ColorRange();
                        colores.Range = reader["Range"].ToString();
                        colores.Color = reader["Color"].ToString();
                        colores.Description = reader["Description"].ToString();
                        Lcolores.Add(colores);
                    }

                }
                listColorRange.ColorRange = Lcolores;
                connection.Close();
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                strSQL = string.Format("CALL {0}.ins_msg_proc('{1}','{2}','{3}')", DataSource.bd(), "APP Sales Force GET", "Error", "Despacho_GetColores - " + ex.Message + " " + imei);
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                connection.Close();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return listColorRange;
        }

        
        public string ColorMargen(double Margen)
        {
            string HexadecimalColor = string.Empty;
            if (Margen <= 24.99)
            {
                HexadecimalColor = "#FF0000"; //ROJO
            }
            else if (Margen >= 30  && Margen <= 34.99)
            {
                HexadecimalColor = "#FFA500"; //NARANJA
            }
            else if (Margen >= 35 && Margen <= 39.99)
            {
                HexadecimalColor = "#FFFF00"; //AMARILLO
            }
            else if (Margen >= 35 && Margen <= 39.99)
            {
                HexadecimalColor = "#00FF00"; //VERDE
            }
            else
            {
                HexadecimalColor = "#0000FF"; //AZUL
            }
            return HexadecimalColor;
        }

        public double CostoProducto(string ItemCode,string Almacen) {

            HanaDataReader reader;
            HanaConnection connection = GetConnection();
            string strSQL =string.Format("SELECT T0.\"AvgPrice\" FROM {0}.OITW T0 " +
                                         "WHERE T0.\"ItemCode\"='{1}' AND T0.\"WhsCode\"='{2}' ", DataSource.bd(),ItemCode,Almacen);
            double Costo = 0;
            try
            {

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

                connection.Open();
                HanaCommand command = new HanaCommand(strSQL, connection);

                reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Costo= Convert.ToDouble(reader["AvgPrice"].ToString());
                       
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return Costo;
        }

        public double GetAvgPrice(string itemCode, string warehouseCode)
        {
            return CostoProducto(itemCode, warehouseCode);
        }

        public List<ColoresOV> CalcularMargen(ColorListSalesOrder colorListSalesOrder)
        {
            var result = colorListSalesOrder.SalesOrders
                .SelectMany(so => so.DocumentLines)
                .GroupBy(dl => dl.U_VIS_PromID)
                .ToDictionary(
                    g => g.Key,
                    g =>
                    {
                        double lineTotal = g.Sum(dl => Convert.ToDouble(dl.LineTotal));
                        double totalQuantity = g.Sum(dl => Convert.ToDouble(dl.Quantity));
                        double avgPrice = GetAvgPrice(
                            g.First().ItemCode,
                            g.First().WarehouseCode); // Obtener el precio promedio

                    double vatPrcnt = g.First().TaxCode == "IGV" ? 18 : 0;

                        double resultado = Math.Round(
                            (lineTotal - (totalQuantity * avgPrice)) /
                            (lineTotal == 0 ? 1 : lineTotal) * 100, 2);

                        return resultado;
                    }
                );

            var SerealJSON_OV = JsonConvert.SerializeObject(result);

            // Parsear el JSON a un objeto JObject
            JObject jsonObj = JObject.Parse(SerealJSON_OV);

            // Crear una lista de diccionarios para almacenar el nuevo formato
            //List<Dictionary<string, object>> nuevoFormato = new List<Dictionary<string, object>>();

            // Crear una lista de objetos ColoresOV
            List<ColoresOV> nuevoFormato = new List<ColoresOV>();

            // Iterar sobre las propiedades del objeto JSON original
            foreach (var propiedad in jsonObj)
            {
                // Crear un nuevo objeto de tipo ColoresOV
                ColoresOV nuevoObj = new ColoresOV
                {
                    Id = propiedad.Key, // Asignar la clave como Id
                    Margen = (double)propiedad.Value, // Asignar el valor como Margen
                    Color = ColorMargen(Convert.ToDouble(propiedad.Value)) // Puedes asignar un color por defecto o manejarlo como desees
                };

                // Agregar el nuevo objeto a la lista
                nuevoFormato.Add(nuevoObj);
            }
            
            
            return nuevoFormato;
        }

        public List<ColoresOV> CalcularMargen2(ColorListSalesOrder colorListSalesOrder)
        {

            // Inicializar los valores totales
            double totalLineTotal = 0;
            double totalQuantity = 0;
            double totalAvgPrice = 0;

            // Recorrer cada SalesOrder y sus DocumentLines
            foreach (var salesOrder in colorListSalesOrder.SalesOrders)
            {
                foreach (var documentLine in salesOrder.DocumentLines)
                {
                    double lineTotal = Convert.ToDouble(documentLine.LineTotal);
                    double quantity = Convert.ToDouble(documentLine.Quantity);
                    double avgPrice = GetAvgPrice(documentLine.ItemCode, documentLine.WarehouseCode); // Obtener el precio promedio

                    totalLineTotal += lineTotal;
                    totalQuantity += quantity;
                    totalAvgPrice += avgPrice * quantity; // Sumar el total de AvgPrice por cantidad
                }
            }

            // Calcular el margen total
            double margenTotal = Math.Round(
                (totalLineTotal - totalAvgPrice) /
                (totalLineTotal == 0 ? 1 : totalLineTotal) * 100, 2);

            // Crear el objeto ColoresOV con el margen total
            ColoresOV resultadoTotal = new ColoresOV
            {
                Id = "Total", // O cualquier otro identificador general
                Margen = margenTotal, // Asignar el margen total calculado
                Color = ColorMargen(margenTotal) // Asignar el color basado en el margen total
            };

            // Devolver una lista con un solo objeto ColoresOV
            return new List<ColoresOV> { resultadoTotal };

        }

        public ColorListSalesOrder PostOrderVentaColores(ColorListSalesOrder colorListSalesOrder)
        {
            // Llamar al método para calcular el margen
           // var results = CalcularMargen(colorListSalesOrder);


            var JsonSalesOrder = JsonConvert.SerializeObject(colorListSalesOrder);

            // Deserializar el JSON en el objeto ColorListSalesOrder
            ColorListSalesOrder ObjcolorListSalesOrder = JsonConvert.DeserializeObject<ColorListSalesOrder>(JsonSalesOrder);

            // Llamar al método para modificar y devolver el objeto
            var updatedColorListSalesOrder = CalcularMargen(ObjcolorListSalesOrder);
            var updatedColorListSalesOrder2 = CalcularMargen2(ObjcolorListSalesOrder);

            // Imprimir el JSON modificado
            var updatedJson = JsonConvert.SerializeObject(updatedColorListSalesOrder);
            var updatedJson2 = JsonConvert.SerializeObject(updatedColorListSalesOrder2);
            // Parsear el segundo JSON a una lista de objetos
            var listaSegundoJson = JsonConvert.DeserializeObject<List<ColoresOV>>(updatedJson);

            var listaSegundoJson2 = JsonConvert.DeserializeObject<List<ColoresOV>>(updatedJson2);

            // Crear un diccionario para buscar rápidamente por Id
            var diccionario = new Dictionary<string, ColoresOV>();
            foreach (var item in listaSegundoJson)
            {
                diccionario[item.Id] = item;
            }

            // Parsear el primer JSON a un objeto JObject
            var jsonPrimeroObj = JObject.Parse(JsonSalesOrder);
            var Cabecera = jsonPrimeroObj["SalesOrders"][0];
            var documentLines = jsonPrimeroObj["SalesOrders"][0]["DocumentLines"];


            // Iterar sobre los DocumentLines y actualizar el ColorApproval
            foreach (var line in documentLines)
            {
                var promId = line["U_VIS_PromID"].ToString();
                if (diccionario.TryGetValue(promId, out var segundoItem))
                {
                    line["ColorApproval"] = segundoItem.Color;
                }
            }

            // Convertir el resultado a JSON
            var jsonResultado = JsonConvert.SerializeObject(jsonPrimeroObj);
            ColorListSalesOrder objcolorListSalesOrder2 = JsonConvert.DeserializeObject<ColorListSalesOrder>(jsonResultado);

            foreach (var cabecera in objcolorListSalesOrder2.SalesOrders)
            {
                foreach (var item in listaSegundoJson2)
                {
                    cabecera.ColorApproval = item.Color;
                }
            }
                //ColorListSalesOrder ObjcolorListSalesOrder2 = JsonConvert.SerializeObject<ColorListSalesOrder>(jsonResultado);


                return objcolorListSalesOrder2;

        }

        #region Disposable




        private bool disposing = false;
        /// <summary>
        /// Método de IDisposable para desechar la clase.
        /// </summary>
        public void Dispose()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        /// <summary>
        /// Método sobrecargado de Dispose que será el que
        /// libera los recursos, controla que solo se ejecute
        /// dicha lógica una vez y evita que el GC tenga que
        /// llamar al destructor de clase.
        /// </summary>
        /// <param name=”b”></param>
        protected virtual void Dispose(bool b)
        {
            // Si no se esta destruyendo ya…
            {
                if (!disposing)

                    // La marco como desechada ó desechandose,
                    // de forma que no se puede ejecutar este código
                    // dos veces.
                    disposing = true;
                // Indico al GC que no llame al destructor
                // de esta clase al recolectarla.
                GC.SuppressFinalize(this);
                // … libero los recursos… 
            }
        }




        /// <summary>
        /// Destructor de clase.
        /// En caso de que se nos olvide “desechar” la clase,
        /// el GC llamará al destructor, que tambén ejecuta la lógica
        /// anterior para liberar los recursos.
        /// </summary>
        ~ColoresDAL()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        #endregion
    }
}
