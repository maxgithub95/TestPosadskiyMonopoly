using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPosadskiyMonopoly
{
    public class WarehouseObjekt
    {
        protected int ID;
        protected double width;
        protected double height;
        protected double length;
        protected double weight;
        protected double volume;
        protected DateOnly ExpirationDate = DateOnly.MaxValue;
        public int GetID() => ID;
        public DateOnly GetED() => ExpirationDate;
        public double GetV() => volume;
        public double GetWe() => weight;
        public double GetWi() => width;
        public double GetH() => height;
        public double GetL() => length;
    }
}
