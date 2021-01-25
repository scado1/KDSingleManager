using System;
using System.Collections.Generic;
using System.Text;

namespace KDSingleManager.Models
{
    public class DefSkladki
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Nazwa { get; set; }
        public decimal ZUS51 { get; set; }
        public decimal ZUS52 { get; set; }
        public decimal ZUS53 { get; set; }

    }
}
