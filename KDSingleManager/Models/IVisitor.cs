using System;
using System.Collections.Generic;
using System.Text;

namespace KDSingleManager.Models
{
    public interface IVisitor
    {
        void visit(Skladka element);
    }
}
