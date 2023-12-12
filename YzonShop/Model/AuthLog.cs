using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzonShop.Model
{
    public class AuthLog
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public DateTime Date { get; set; }
        public bool Access {  get; set; }

        public AuthLog(int id, string login, DateTime date, bool access)
        {
            Id = id;
            Login = login;
            Date = date;
            Access = access;
        }
    }
}
