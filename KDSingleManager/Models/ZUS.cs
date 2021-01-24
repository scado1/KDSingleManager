using System;
using System.Collections.Generic;
using System.Text;

namespace KDSingleManager.Models
{
    public class ZUS
    {
        public int ZusId { get; set; }
        public int year { get; set; }
        public decimal Total { get; set; }
        public int IdSubcontractor { get; set; }
        public virtual Subcontractor Subcontractor { get; set; }
    }
}
