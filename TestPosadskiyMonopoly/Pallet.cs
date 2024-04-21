using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPosadskiyMonopoly
{
    public class Pallet : WarehouseObjekt
    {
        List<Box> boxes = new List<Box>();
        
        public Pallet(double wi, double h, double l)
        {
            ID = Program.GenerateID();
            width = wi;
            height = h;
            length = l;
            weight = 30;
            volume = wi * h * l;
        }
        public bool PutBox(Box box)
        {
            if ((box.GetWi() > width) || (box.GetL() > length))
            {
                Console.WriteLine("Слишком большая коробка для данного палета, создайте коробку заново!");
                return false;
            }
            else
            {
                boxes.Add(box);
                if (box.GetED().CompareTo(ExpirationDate) < 0)
                {
                    ExpirationDate = box.GetED();
                }
                volume += box.GetV();
                weight += box.GetWe();
                return true;
            }
        }
        public bool RemoveBox(Box box)
        {
            if (boxes.Remove(box)) { 
            if (box.GetED().CompareTo(ExpirationDate) == 0)
                {
                    DateOnly min = DateOnly.MaxValue;
                    foreach (Box item in boxes) 
                    {
                        if (item.GetED().CompareTo(min) < 0) min = item.GetED();
                    }
                    ExpirationDate = min;
                }
                volume -= box.GetV();
                weight -= box.GetWe();
                return true;
            }
            else
            {
                Console.WriteLine("Данной коробки не нашлось в этой палете");
                return false;
            }            
        }
        
        public List<Box> GetBoxses() => boxes;
    }
}
