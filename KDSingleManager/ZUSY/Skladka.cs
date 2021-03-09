using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace KDSingleManager.Models
{
    public class Skladka : IComponent
    {
        public int Id { get; set; }
        public virtual DefSkladki DefSkladka { get; set; }
        public virtual Subcontractor Subcontractor { get; set; }
        public int ZaOkresMonth { get; set; }
        public int ZaOkresYear { get; set; }
        public int Stan { get; set; } 
        public string Data { get; set; }
        public string Opis { get; set; }
        public decimal Wartość { get; set; }
        //public decimal FxRate { get; set; }
        public void Accept(IVisitor visitor)
        {
            visitor.visit(this);
        }
    }
}
