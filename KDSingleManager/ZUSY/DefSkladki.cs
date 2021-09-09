using KDSingleManager.ZUSY;
using System;
using System.Collections.Generic;
using System.Text;

namespace KDSingleManager.Models
{
    public class DefSkladki : IZUS
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Nazwa { get; set; }
        public int Year { get; set; }
        //public virtual Skladka Skladka { get; set; }
        public decimal ZUS51 { get; set; }
        public decimal ZUS52 { get; set; }
        public decimal ZUS53 { get; set; }

        public decimal Get51()
        {
            return this.ZUS51;
        }

        public decimal Get52()
        {
            return this.ZUS52;
        }

        public string GetNazwa()
        {
            return this.Nazwa;
        }

        public decimal GetWartosc()
        {
            return this.ZUS51 + this.ZUS52 + this.ZUS53;
        }
    }
}
