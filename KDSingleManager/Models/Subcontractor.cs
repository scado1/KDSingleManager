using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace KDSingleManager.Models
{
    public class Subcontractor
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DataZalozenia { get; set; }
        //  public virtual ICollection<Skladka> ZUSy { get; set; } = new ObservableCollection<Skladka>();
        public virtual ICollection<Przejscie> Przejscia { get; set; } = new ObservableCollection<Przejscie>();
        public virtual ICollection<Skladka> Skladki { get; set; } = new ObservableCollection<Skladka>();
    }
}
