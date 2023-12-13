using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace YzonShop.Model
{
    public class Shop
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public bool DeliverPay {  get; set; }
        public Shop()
        {
            EmailAddress = string.Empty;
        }
        public Shop(int id, string emailAddress, bool deliverPay)
        {
            Id = id;
            EmailAddress = emailAddress;
            DeliverPay = deliverPay;
        }

        public override string ToString() => $"{EmailAddress}; {DeliverPay}";
    }
}