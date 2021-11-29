using System;
using System.Collections.Generic;
using System.Text;

namespace KDSingleManager.Models
{
    public class Renumeration : IComponent
    {
        public int Id { get; set; }
        public virtual Subcontractor Subcontractor { get; set; }
        public int ZaOkresMoth { get; set; }
        public int ZaOkresYear { get; set; }
        public int Stan { get; set; }
        public string Data { get; set; }
        public string Opis { get; set; }
        public decimal Wartosc { get; set; }
        public decimal Tax { get; set; }

        public void Accept(IVisitor visitor)
        {
            visitor.visit(this);
        }

    }
}
