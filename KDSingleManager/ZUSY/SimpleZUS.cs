using KDSingleManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace KDSingleManager.ZUSY
{

    public class SimpleZUS : ISimpleZUS
    {
        AppContext _context = MainWindow._context;
        public override IZUS AddZUS(string b1)
        {
            //var _context = MainWindow._context;
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

        private DateTime Convert(string d) => DateTime.Parse(d);
        public override IZUS AddZUS(Subcontractor s, int month, int year)
        {
            IZUS intendedZUS = null;
            //Should recieve data from ...?
            // DateTime OkresZUS = DateTime.Parse("2020-10-10");

            DateTime OkresZUS = new DateTime(year, month,2);

            DateTime pref = Convert(s.DataZalozenia);
            DateTime maly, duzy;

            maly = Convert(s.Przejscia.Select(x => x.PrzejscieNaMaly).FirstOrDefault().ToString());
            //DateTime.Parse(s.Przejscia.Select(x => x.PrzejscieNaMaly).FirstOrDefault().ToString());
            duzy = Convert(s.Przejscia.Select(x => x.PrzejscieNaDuzy).FirstOrDefault().ToString());
            //DateTime.Parse(s.Przejscia.Select(x => x.PrzejscieNaDuzy).FirstOrDefault().ToString());

            string nazwa = string.Empty;

            var x = ((year - pref.Year) * 12) + month - pref.Month;


            if (OkresZUS > duzy || (year == duzy.Year && month == duzy.Month))
            {
                //MessageBox.Show($"OkresZUS > duzy {OkresZUS > duzy} {OkresZUS - duzy}");
                nazwa = "Duży";
            }
            else if (OkresZUS > maly || (year == maly.Year && month == maly.Month))
            {
                //MessageBox.Show($"OkresZUS > dMaly {OkresZUS > maly} {OkresZUS - maly}");
                nazwa = "Mały";
            }
            else if (OkresZUS >= pref || (year == pref.Year && month == pref.Month))
            {
                //MessageBox.Show($"OkresZUS > dDG {OkresZUS > pref} {OkresZUS - pref}");
                nazwa = "Ulga";
            }
            else
            {
                throw new Exception("Data DG > Data ZUS");
            }

            intendedZUS = _context.DefinicjeSkladek.Where(x => x.Nazwa == nazwa && x.Year == year).FirstOrDefault();

            MessageBox.Show(string.Format($"{intendedZUS.GetWartosc()} {intendedZUS.GetNazwa()}"));
            return intendedZUS;
        }

    }
}
