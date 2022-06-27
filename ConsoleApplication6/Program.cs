using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleApplication5
{

    enum Rank
    {
        Soldier,
        Commander,
        Leutenant,
        Captain,
        General
    }

    struct Name
    {
        public string first;
        public string last;
    }

    struct Statistics
    {
        public Rank rank;
        public Order prevOrder;
        public Order newOrder;
    }

    struct Order
    {
        public string order;
    }

    class Military
    {
        Name name;
        Rank rank;
        Order order;


        public Military(Random rand)
        {
            Generate(ref name, ref rank, ref order, rand);
        }

        public Military(Name n, Rank r, Order o)
        {
            name = n;
            rank = r;
            order = o;
        }

        public void SetName(Name n)
        {
            name = n;
        }

        public Name GetName()
        {
            return name;
        }

        public void SetRank(Rank r)
        {
            rank = r;
        }

        public Rank GetRank()
        {
            return rank;
        }

        public void SetOrder(Order o)
        {
            order = o;
        }

        public Order GetOrder()
        {
            return order;
        }

        public void Generate(ref Name n, ref Rank r, ref Order o, Random rand)
        {
            string[] names = { "Арсений", "Владимир", "Егор", "Кирилл", "Артём", "Григорий", "Глеб", "Леон", "Лев", "Илья" };
            string[] last_names = { "Федотов", "Титов", "Ларин", "Копылов", "Березин", "Дмитриев", "Никифоров", "Иванов", "Максимов", "Федоров" };
            string[] orders = { "Приказ 1", "Приказ 2", "Приказ 3", "Приказ  4", "Приказ 5" };

            n.first = names[rand.Next(names.Length)];
            n.last = last_names[rand.Next(last_names.Length)];

            r = (Rank)rand.Next(5);

            o.order = orders[rand.Next(orders.Length)];
        }
    }

    static class Contact
    {
        static Rank orderRank;
        static Order order;
        static List<Statistics> statistics;

        static Contact()
        {
            orderRank = new Rank();
            order = new Order();
            statistics = new List<Statistics>();
        }

        public static void Enter(Military military)
        {
            Statistics stat = new Statistics();
            Order tempOrder;

            stat.rank = military.GetRank();
            stat.prevOrder = order;

            if (military.GetRank() < orderRank && order.order != null && order.order != string.Empty)
            {
                military.SetOrder(order);

                order = new Order();
                orderRank = new Rank();
                stat.newOrder.order = string.Empty;
            }
            else if (military.GetRank() == orderRank && order.order != null && order.order != string.Empty)
            {
                stat.newOrder = order;
            }
            else
            {
                stat.newOrder = military.GetOrder();
                tempOrder = military.GetOrder();
                military.SetOrder(new Order());
                order = tempOrder;
                orderRank = military.GetRank();
            }

            statistics.Add(stat);
        }

        public static List<Statistics> GetStats()
        {
            return statistics;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Military> militaries = new List<Military>();
            List<Statistics> statistics;
            Dictionary<Rank, int> ranks = new Dictionary<Rank, int> {{Rank.Soldier, 0}, {Rank.Commander, 0}, {Rank.Leutenant, 0}, {Rank.Captain, 0}, {Rank.General, 0}};
            Random rand = new Random();

            UInt32 count = 0;

            Console.Write("Количество: ");
            try
            {
                count = Convert.ToUInt32(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Ошибка ввода.");
            }

            for (int i = 0; i < count; i++)
            {
                militaries.Add(new Military(rand));
            }

            foreach (Military military in militaries)
            {
                Console.WriteLine("Имя: {0} {1}, ранг: {2}, приказ до: {3}", military.GetName().first, military.GetName().last, military.GetRank(), (military.GetOrder().order != null && military.GetOrder().order != string.Empty ? military.GetOrder().order : "нет приказа" ));
            }

            foreach (Military military in militaries)
            {
                Contact.Enter(military);
            }

            Console.WriteLine("");
            foreach (Military military in militaries)
            {
                Console.WriteLine("Имя: {0} {1}, ранг: {2}, приказ после: {3}", military.GetName().first, military.GetName().last, military.GetRank(), (military.GetOrder().order != null && military.GetOrder().order != string.Empty ? military.GetOrder().order : "нет приказа"));
            }

            statistics = Contact.GetStats();

            Console.WriteLine("");
            foreach (Statistics stat in statistics)
            {
                Console.WriteLine("Ранг: {0}, предыдущий приказ: {1}, текущий приказ: {2}", stat.rank, (stat.prevOrder.order != null && stat.prevOrder.order != string.Empty ? stat.prevOrder.order : "нет приказа"), (stat.newOrder.order != null && stat.newOrder.order != string.Empty ? stat.newOrder.order : "нет приказа"));
                ranks[stat.rank] += 1;
            }

            Console.WriteLine("");
            foreach (Rank rank in ranks.Keys)
            {
                Console.WriteLine("{0} : {1}", rank, ranks[rank]);
            }

            Console.ReadKey();
        }
    }
}
