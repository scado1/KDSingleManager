using KDSingleManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KDSingleManager.ZUSY
{
    public class SimpleZUS : ISimpleZUS
    {
        public override IZUS AddZUS(string b1)
        {
            var _context = MainWindow._context;
            IZUS intendedZUS = null;

            Console.WriteLine("enter the date:");
            //string b1 = Console.ReadLine();
            DateTime dt;
            int diff;

            if (DateTime.TryParse(b1, out dt))
            {
                diff = ((DateTime.Now.Year - dt.Date.Year) * 12) + DateTime.Now.Month - dt.Date.Month;
                Console.WriteLine(diff);

                intendedZUS = (DefSkladki)_context.DefinicjeSkladek.Where(x => x.Year == dt.Year).FirstOrDefault();
            }

            return intendedZUS;
        }

        public override IZUS AddZUS()
        {
            throw new NotImplementedException();
        }
    }
}
