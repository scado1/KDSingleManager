using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace KDSingleManager.Models
{
    public class WynagrKonto
    {
        public int Id { get; set; }
        public virtual Subcontractor Subcontractor { get; set; }
        public string Konto { get; set; }
        public string Swift { get; set; }
        public string Kraj { get; set; } = "PL";
        public string PosiadaczRachunku { get; set; }
    }
}
