using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLHelperLib.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }

        public User()
        {
            Name = string.Empty;
            Phone = string.Empty;
        }

        public User(int id, string name, string phone)
        {
            Id = id;
            Name = name;
            Phone = phone;
        }

        public override string ToString() => $"{Name}; {Phone}";
    }
}
