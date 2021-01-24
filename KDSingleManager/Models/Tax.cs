using System;
using System.Collections.Generic;
using System.Text;

namespace KDSingleManager.Models
{
    public class Tax
    {
        public int IdTax { get; set; }
        public int year { get; set; }
        public decimal Total { get; set; }
        public decimal DoOdliczenia { get; set; }
        public int IdSubcontractor { get; set; }
        public virtual Subcontractor Subcontractor { get; set; }
    }
}
