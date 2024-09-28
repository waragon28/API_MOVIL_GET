using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_Core.BO
{
    public class ListaPreciosBO
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Uom { get; set; }
        public decimal WhsStock { get; set; }
        public decimal StockTotal { get; set; }
        public decimal Cash { get; set; }
        public decimal Credit { get; set; }
        public decimal Gallons { get; set; }
        public decimal Liter { get; set; }
        public string OilTax { get; set; }
        public string SIGAUS { get; set; }
        public string Type { get; set; }
        public decimal DiscPrcnt { get; set; }
        public string CashDscnt { get; set; }
        public int Units { get; set; }
        public string MonedaAdicional { get; set; }
        public decimal MonedaAdicionalContado { get; set; }
        public decimal MonedaAdicionalCredito { get; set; }  
        public int CodePriceListCash { get; set; }
        public int CodePriceListCredit { get; set; }
        public string Inventariable { get; set; }
    }
    public class ListaPrecios
    {
        public List<ListaPreciosBO> PriceList { get; set; }
    }
    public class ListaPreciosHeaderBO
    {
        public int ListNum { get; set; }
        public string ListName { get; set; }
        public decimal PrcntIncrease { get; set; }
    }
    public class ListaPreciosHeader
    {
        public List<ListaPreciosHeaderBO> PriceListHeader { get; set; }
    }
    public class LPWarehouseBO
    {
        public string WhsCode { get; set; }
        public string WhsName { get; set; }
        public int PriceListCash { get; set; }
        public int PriceListCredit { get; set; }
        public string U_VIST_SUCUSU { get; set; }
    }
    public class LPListWarehouse
    {
        public List<LPWarehouseBO> WarehouseList { get; set; }
    }

    public class ListaPreciosWhsBO
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string UDM { get; set; }
        public decimal Disponible { get; set; }
        public string CodAlmacen { get; set; }
        public decimal Contado { get; set; }
        public decimal Credito { get; set; }
        public string Tipo { get; set; }
        public decimal DiscPrcnt { get; set; }
        public decimal GAL { get; set; }
        public decimal StockTotal { get; set; }
        public string CashDscnt { get; set; }
        public int Units { get; set; }
        public string OilTax { get; set; }
        public decimal Liter { get; set; }
        public string SIGAUS { get; set; }
        public string MonedaAdicional { get; set; }
        public decimal MonedaAdicionalContado { get; set; }
        public decimal MonedaAdicionalCredito { get; set; }
        public int CodePriceListCash { get; set; }
        public int CodePriceListCredit { get; set; }
    }
    public class ListaPreciosWhs
    {
        public List<ListaPreciosBO> PriceList { get; set; }
    }
}
