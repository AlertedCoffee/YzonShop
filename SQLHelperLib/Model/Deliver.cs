using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLHelperLib.Model
{
        public class Deliver
        {
            public Order Order {  get; set; }
            public DateTime Date { get; set; }
            public string Address { get; set; }
            public Deliver()
            {
                Address = string.Empty;
            }

            public Deliver(Order order, DateTime date, string address)
            {
                Order = order;
                Date = date;
                Address = address;
            }
        }
}
