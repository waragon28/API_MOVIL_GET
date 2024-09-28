using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace SAP_Core.BO
{
    public class CobranzaDBO
    {        
        public string Code { get; set; }
        public string DocEntry { get; set; }
        public string BankID { get; set; }
        public string DepositID { get; set; }
        public string ItemDetail { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string LegalNumber { get; set; }
        public string DocEntryFT { get; set; }
        public string DocNum { get; set; }
        public decimal DocTotal { get; set; }
        public decimal Balance { get; set; }
        public decimal AmountCharged { get; set; }
        public decimal NewBalance { get; set; }
        public string IncomeDate { get; set; }
        public int Receip { get; set; }
        public string Status { get; set; }
        public string Commentary { get; set; }
        public string SlpCode { get; set; }
        public string UserCode {get;set;}
        public string Banking { get; set; }
        public string QRStatus { get; set; }
        public string CancellationReason { get; set; }
        public string DirectDeposit { get; set; }
        public string POSPay { get; set; }
    }
    public class ListaCobranzaD
    {
        public List<CobranzaDBO> CollectionDetail { get; set; }
    }
}
