using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace KDSingleManager.Models
{
    public class Mikrorachunek
    {
        public int Id { get; set; }
        [Unique]
        public virtual Subcontractor Subcontractor { get; set; }
        public string Konto { get; set; }
    }
}
