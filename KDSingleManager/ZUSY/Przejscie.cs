using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace KDSingleManager.Models
{
    public class Przejscie
    {
        public int Id { get; set; }
        [Unique]
        public virtual Subcontractor Subcontractor { get; set; }
        public string PrzejscieNaMaly { get; set; }
        public string PrzejscieNaDuzy { get; set; }

        //public string Definicja { get; set; }
        //public IList<DateTime> Daty { get; set; }
        //public Przejscie(string data)
        //{
        //    Daty = new List<DateTime>();
        //    Definicja = data;
        //}

        //public static Tuple<Definicja, DateTime> _przejscie { get; set; }
        //public enum Definicja { NaMały, NaDuży };

    };
}

