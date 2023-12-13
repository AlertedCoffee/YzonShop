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
        public int Goods { get; set; }
        public int Shop { get; set; }
        public DateTime OrderDate { get; set; }
        public int Count { get; set; }
        public int ClientId { get; set; }
        public bool Apply {  get; set; }

        public Order() { }

        public Order(int id, int goods, int shop, DateTime orderDate, int count, int clientId, bool apply)
        {
            Id = id;
            Goods = goods;
            Shop = shop;
            OrderDate = orderDate;
            Count = count;
            ClientId = clientId;
            Apply = apply;
        }

        public override string ToString() => $"{Id}";

    }
}
