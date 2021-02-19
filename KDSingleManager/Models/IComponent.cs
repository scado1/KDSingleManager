using System;
using System.Collections.Generic;
using System.Text;

namespace KDSingleManager.Models
{
    public interface IComponent
    {
        void Accept(IVisitor visitor);
    }
}
