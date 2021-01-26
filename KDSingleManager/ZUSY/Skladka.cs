using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace KDSingleManager.Models
{
    public class Skladka
    {
        public int Id { get; set; }
        public virtual DefSkladki DefSkladki { get; set; }
        public int ZaOkresMonth { get; set; }
        public int ZaOkresYear { get; set; }
        public int Stan { get; set; }
        public string Data { get; set; }
        public string Opis { get; set; }
        public decimal Wartość { get; set; }
    }
}
