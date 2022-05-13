using ClosedXML.Excel;
using KDSingleManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KDSingleManager.Processors
{
   public class InvoiceProcessor
    {
        //public static void ExtractInvoices (string fp, xlWorkSheet name, out List<InvoiceExport> Faktury)
        public static void ExtractInvoices (string filePath, IXLWorksheet workSheet, out List<InvoiceExport> Faktury)
        {
            Faktury = new List<InvoiceExport>();

            List<string> firstName = new List<string>();
            List<string> lastName = new List<string>();

            List<string> NIP = new List<string>();

            List<decimal> sumEUR = new List<decimal>();
            List<decimal> sumPLN = new List<decimal>();

            var wb = new XLWorkbook(filePath);
            var ws = workSheet;

            var firstRowUsed = ws.FirstRowUsed();

            var categoryRow = firstRowUsed.RowUsed();
            categoryRow = categoryRow.RowBelow();

            var firstPossibleAddress = ws.Row(categoryRow.RowNumber()).FirstCell().Address;
            var lastPossibleAddress = ws.LastCellUsed().Address;

            var PPERange = ws.Range(firstPossibleAddress, lastPossibleAddress).RangeUsed();

            var PPETable = PPERange.AsTable();

            firstName = PPETable.DataRange.Rows().Select(x => x.Field("Name").GetString()).ToList();
            lastName = PPETable.DataRange.Rows().Select(x => x.Field("Surname").GetString()).ToList();

            NIP = PPETable.DataRange.Rows().Select(x => x.Field("NIP ").GetString()).ToList();

            sumEUR = PPETable.DataRange.Rows().Select(x => decimal.Parse(x.Field("SUM EUR").GetString())).ToList();
            sumPLN = PPETable.DataRange.Rows().Select(x => decimal.Parse(x.Field("Przychód KD").GetString())).ToList();

            var color = PPETable.DataRange.Rows().Select(x => x.Field("Surname").Style.Fill.BackgroundColor.Color.Name).ToList();

            for (int i = 0; i < NIP.Count; i++)
            {

                Faktury.Add
                (
                    new InvoiceExport
                    {
                        Kontrahent = new Subcontractor
                        {
                            FirstName = firstName[i].TrimEnd().TrimStart(),
                            LastName = lastName[i].TrimEnd().TrimStart(),
                            NIP = NIP[i].TrimEnd().TrimStart(),
                        },
                        KwotaEUR = sumEUR[i],
                        KwotaPLN = sumPLN[i]
                    }
                );
            }

            foreach (var item in Faktury.ToList())
            {
                if (item.KwotaEUR == 0m && item.KwotaPLN == 0m)
                {
                    Faktury.Remove(item);
                }
            }
        }
    }
}
