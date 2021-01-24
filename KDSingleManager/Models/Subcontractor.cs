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
        public virtual ICollection<ZUS> ZUSy { get; set; } = new ObservableCollection<ZUS>();
    }
}
