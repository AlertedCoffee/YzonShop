using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLHelperLib.Model
{
    public class Goods
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Firm {  get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public float Cost { get; set; }
        public int Warranty { get; set; }
        public string Image { get; set; }

        public Goods(int id, string name, string firm, string model, string description, float cost, int warranty, string image)
        {
            Id = id;
            Name = name;
            Firm = firm;
            Model = model;
            Description = description;
            Cost = cost;
            Warranty = warranty;
            Image = image;
        }

        public Goods() {
            Name = "";
            Firm = "";
            Model = "";
            Description = "";
            Image = "";
        }

        public override string ToString() => $"{Name}; {Firm}; {Model}";
    }
}
