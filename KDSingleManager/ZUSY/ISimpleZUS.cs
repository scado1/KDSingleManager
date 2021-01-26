using KDSingleManager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KDSingleManager.ZUSY
{
    public abstract class ISimpleZUS
    {
        public abstract IZUS AddZUS();
        public abstract IZUS AddZUS(string dt);
        public abstract IZUS AddZUS(Subcontractor s);
    }
}
