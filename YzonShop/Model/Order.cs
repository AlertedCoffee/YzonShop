using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzonShop.Model
{
    public class Order
    {
        public int Id { get; set; }
        public Goods Goods { get; set; }
        public Shop Shop { get; set; }
        public DateTime OrderDate { get; set; }
        public int Count { get; set; }
        public User Client { get; set; }
        public bool Apply {  get; set; }

        public Order() { }

        public Order(int id, Goods goods, Shop shop, DateTime orderDate, int count, User client, bool apply)
        {
            Id = id;
            Goods = goods;
            Shop = shop;
            OrderDate = orderDate;
            Count = count;
            Client = client;
            Apply = apply;
        }

        public override string ToString() => $"{Id}";

    }
}
