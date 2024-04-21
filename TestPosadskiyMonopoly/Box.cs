using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPosadskiyMonopoly
{
    public class Box : WarehouseObjekt
    {
        public enum typeDate
        {
            ED,
            PD
        }
        public Box(double wi, double h, double l, double we, typeDate type, DateOnly date)
        {
            ID = Program.GenerateID();
            width = wi;
            height = h;
            length = l;
            weight = we;
            if (type == typeDate.ED)
                ExpirationDate = date;
            else
                ExpirationDate = date.AddDays(100);
            volume = wi * h * l;
        }               
        
    }
}
