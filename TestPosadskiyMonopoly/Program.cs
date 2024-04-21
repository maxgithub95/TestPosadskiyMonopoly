using System.Collections.Immutable;
using System.Linq;

namespace TestPosadskiyMonopoly
{
    internal class Program
    {
        private static int ID = 0;
        public static int GenerateID()
        {
            ID++;
            return ID;
        }

        private static void GeneratePallets(ref Dictionary<int, Pallet> pallets)
        {
            Console.WriteLine("Создание паллет");
            int count = 1;
            bool dontStop = true;
            while (dontStop)
            {
                Console.WriteLine("Хотите добавить паллету? (да/нет)");
                string? answer = Console.ReadLine();
                switch (answer)
                {
                    case "да":
                        Pallet p = GeneratePallet(count);
                        pallets.Add(p.GetID(), p);
                        Console.WriteLine($"Паллета №{count} успешно создана!");
                        count++;
                        break;
                    case "нет":
                        dontStop = false;
                        break;
                    default:
                        Console.WriteLine("Некорректный ответ");
                        break;
                }
            }
        }
        private static Pallet GeneratePallet(int count)
        {            
                Console.WriteLine($"Введите параметры палеты №{count}:");
                Console.Write("width = ");
                double wi = Convert.ToDouble(Console.ReadLine());
                Console.Write("\r\nheight = ");
                double h = Convert.ToDouble(Console.ReadLine());
                Console.Write("\r\nlength = ");
                double l = Convert.ToDouble(Console.ReadLine());
                return new Pallet(wi, h, l);                         
        }
        private static Box GenerateBox(int count)
        {
            Console.WriteLine($"Введите параметры коробки №{count}:");
            Console.Write("width = ");
            double wi = Convert.ToDouble(Console.ReadLine());
            Console.Write("\r\nheight = ");
            double h = Convert.ToDouble(Console.ReadLine());
            Console.Write("\r\nlength = ");
            double l = Convert.ToDouble(Console.ReadLine());
            Console.Write("\r\nweight = ");
            double we = Convert.ToDouble(Console.ReadLine());
            bool dontStop = true;
            Box.typeDate type = Box.typeDate.ED;
            DateOnly date = DateOnly.MaxValue;
            while (dontStop) 
            {
                Console.WriteLine("\r\nВыберите вариант, в ответ напишите 1 или 2:" +
                "\r\n 1 - если хотите ввести срок годности," +
                "\r\n 2 - если хотите ввести дату производства");                
                string? answer = Console.ReadLine();
                switch (answer)
                {
                    case "1":
                        type = Box.typeDate.ED;
                        Console.WriteLine($"Введите срок годности");
                        date = DateOnly.FromDateTime(Convert.ToDateTime(Console.ReadLine()));
                        dontStop= false;
                        break;
                    case "2":
                        type = Box.typeDate.PD;
                        Console.WriteLine($"Введите дату производства");
                        date = DateOnly.FromDateTime(Convert.ToDateTime(Console.ReadLine()));
                        dontStop = false;
                        break;
                    default:
                        Console.WriteLine("Некорректный ответ");
                        break;
                }
            }            
            return new Box(wi, h, l, we, type, date);
        }
        private static void GenerateBoxes(ref Dictionary<int, Pallet> pallets)
        {
            Console.WriteLine("Создание коробок");
            int count = 1;
            bool dontStop = true;
            while (dontStop)
            {
                Console.WriteLine("Хотите добавить коробку? (да/нет)");
                Box box;
                string? answer = Console.ReadLine();
                switch (answer)
                {
                    case "да":
                        box = GenerateBox(count);
                        Console.WriteLine($"Коробка №{count} успешно создана! Введите ID паллеты из списка ниже, в которую хотите сложить эту коробку.");
                        foreach (Pallet p in pallets.Values)
                        {
                            Console.Write(p.GetID() + " ");
                        }
                        Console.WriteLine();
                        int IDtoPut = Convert.ToInt32(Console.ReadLine());
                        bool sucsess = pallets[IDtoPut].PutBox(box);
                        if (sucsess) count++;
                        break;
                    case "нет":
                        dontStop = false;
                        break;
                    default:
                        Console.WriteLine("Некорректный ответ");
                        break;
                }
            }
        }
        private static void PrintPallets(Dictionary<int, Pallet> pallets)
        {
            foreach (Pallet p in pallets.Values)
            {
                Console.WriteLine($"Паллета №{p.GetID()}:");
                if (p.GetBoxses().Count == 0) Console.WriteLine("Нет коробок");
                else
                {
                    Console.Write("Коробки: ");
                    foreach (Box box in p.GetBoxses())
                    {
                        Console.Write(box.GetID() + " ");
                    }
                    Console.WriteLine();
                }
            }
        }

        private static void Sorting1(Dictionary<int, Pallet> pallets)
        {
            SortedDictionary<DateOnly, List<Pallet>> Groups = new SortedDictionary<DateOnly, List<Pallet>>();
            bool sucsess;
            foreach (Pallet p in pallets.Values)
            {
                sucsess = false;
                foreach (DateOnly date in Groups.Keys)
                {
                    if (p.GetED().CompareTo(date) == 0)
                    {
                        Groups[date].Add(p);
                        sucsess = true;
                        break;
                    }
                }
                if (!sucsess)
                {
                    Groups.Add(p.GetED(), new List<Pallet>());
                    Groups[p.GetED()].Add(p);
                }
            }
            SortedDictionary<DateOnly, List<Pallet>> GroupsWithSortWeight = new SortedDictionary<DateOnly, List<Pallet>>();
            foreach (DateOnly date in Groups.Keys)
            {
                GroupsWithSortWeight.Add(date, Groups[date].OrderBy(x => x.GetWe()).ToList());
            }
            foreach (DateOnly date in GroupsWithSortWeight.Keys)
            {
                Console.WriteLine(date + ": ");
                foreach (Pallet p in GroupsWithSortWeight[date])
                {
                    Console.WriteLine($"{p.GetID()} {p.GetWe()}");
                }
            }
        }
        private static void Sorting2(Dictionary<int, Pallet> pallets)
        {
            List<(Pallet, DateOnly)> MaxEDOfPalletSort2 = new List<(Pallet, DateOnly)> ();
            DateOnly MaxED;
            foreach (Pallet p in pallets.Values)
            {
                MaxED = DateOnly.MinValue;
                foreach (Box box in p.GetBoxses())
                {
                    if (box.GetED().CompareTo(MaxED)>0) MaxED=box.GetED();
                }
                MaxEDOfPalletSort2.Add((p, MaxED));
            }
            MaxEDOfPalletSort2 = MaxEDOfPalletSort2.OrderByDescending(x => x.Item2).Take(3).OrderBy(x=>x.Item1.GetV()).ToList();
            Console.WriteLine("Результат второй сортировки:");
            foreach ((Pallet, DateOnly) item in MaxEDOfPalletSort2)
            {
                Console.WriteLine($"Палета с ID: {item.Item1.GetID()}, макс. СГ коробки: {item.Item2}, Объём: {item.Item1.GetV()}") ;
            }
        }


        static void Main()
        {
            Dictionary<int, Pallet> pallets= new Dictionary<int, Pallet>();
            GeneratePallets(ref pallets);
            GenerateBoxes(ref pallets);
            PrintPallets(pallets);
            Sorting1(pallets);            
            Sorting2(pallets);
        }

        
    }
}