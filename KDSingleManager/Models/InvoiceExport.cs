using System;
using System.Collections.Generic;
using System.Text;

namespace KDSingleManager.Models
{
    public class InvoiceExport
    {
        public Subcontractor Kontrahent { get; set; }
        public decimal KwotaEUR { get; set; }
        public decimal KwotaPLN { get; set; }
    }
}
