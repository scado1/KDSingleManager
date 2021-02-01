using KDSingleManager.Models;
using KDSingleManager.ZUSY;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KDSingleManager.Processors
{
    public class Processor
    {
        //   private Subcontractor _subccontractor;
        private AppContext _context;
        public void AddZus(Subcontractor s)
        {
            _context = MainWindow._context;
            DateTime today = DateTime.Now;
            Subcontractor _subccontractor = s;

            Skladka skl = new Skladka();
            skl.Data = today.ToShortDateString();
            skl.ZaOkresYear = today.Year;
            skl.ZaOkresMonth = today.Month;
            ISimpleZUS simpleZUS = new SimpleZUS();
            IZUS zUS = simpleZUS.AddZUS(s);

            skl.DefSkladka = (DefSkladki)zUS;
            skl.Wartość = skl.DefSkladka.GetWartosc();
            skl.Subcontractor = s;
            //s.ZUSy.Add(skl);

            _context.Skladki.Add(skl);
            _context.SaveChanges();
        }
    }
}
