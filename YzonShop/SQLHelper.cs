using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YzonShop.Model;

namespace YzonShop
{
    public class SQLHelper
    {
        public SqlConnection Connection;
        private string connectionString = @" Data Source=ALERTEDCOFFEE\SQLEXPRESS;Initial Catalog=EShop;Integrated Security=True; ";
        
        private static SQLHelper _SQLHelper;
        private SQLHelper()
        {
            Connection = new SqlConnection(connectionString);
            Connection.Open();
        }

        public static SQLHelper GetSQLHelper()
        {
            if (_SQLHelper == null) _SQLHelper = new SQLHelper();
            return _SQLHelper;
        }

        private SqlCommand UseQuery(string Query)
        {
            return new SqlCommand(Query, Connection);
        }

        public string[] Login(string login, string password)
        {
            if (login == null || login == "" || password == null || password == "") { throw new Exception("Поля логина и пароля должны быть заполнены"); }

            string Query = $" Select Roles.role, Users.id from Users, Roles where [password] = '{password}' and [login] = '{login}' and Users.role_id = Roles.id";
            string[] result = new string[2];

            SqlDataReader reader = UseQuery(Query).ExecuteReader();
            while (reader.Read())
            {
                result[0] = reader[0].ToString();
                result[1] = reader[1].ToString();
            }
            reader.Close();

            return result;
        }

        public void SaveAuthLog(string login, int access)
        {
            string Query = $"insert into AuthLog values ('{login}', '{DateTime.Now}', {access})";
            UseQuery(Query).ExecuteNonQuery();
        } 


        public List<AuthLog> GetAuthList()
        {
            string query = $"select * from AuthLog";

            return AuthListReturner(query);
        }

        public List<AuthLog> GetSortedAuthList(string login, DateTime date)
        {
            string query;
            if (string.IsNullOrEmpty(login) && date == DateTime.MinValue)
            {
                return GetAuthList();
            }
            else if (string.IsNullOrEmpty(login))
            {
                query = $"select * from AuthLog where date >= '{date.ToString()}' and date < '{date.AddHours(24).ToString()}'";
            }
            else if (date == DateTime.MinValue)
            {
                query = $"select * from AuthLog where login = '{login.ToString()}'";
            }
            else
            {
                query = $"select * from AuthLog where date >= '{date.ToString()}' and date < '{date.AddHours(24).ToString()}' and login = '{login.ToString()}'";
            }

            return AuthListReturner(query);
        }

        private List<AuthLog> AuthListReturner(string query)
        {
            var list = new List<AuthLog>();

            SqlDataReader reader = UseQuery(query).ExecuteReader();
            while (reader.Read())
            {
                list.Add(new AuthLog(Int32.Parse(reader[0].ToString()),
                    reader[1].ToString(),
                    DateTime.Parse(reader[2].ToString()),
                    Boolean.Parse(reader[3].ToString()))
                    );
            }
            reader.Close();

            return list;
        }

        public List<Goods> GetGoods()
        {
            string query = $"select * from Goods";

            return GoodsListReturner(query);

        }

        public void SetGoods(Goods goods)
        {
            string query = $"insert into Goods values ('{goods.Name}', '{goods.Firm}', '{goods.Model}', '{goods.Description}', {goods.Cost.ToString().Replace(',', '.')}, {goods.Warranty}, '{goods.Image}')";
            UseQuery(query).ExecuteNonQuery();
        }

        private List<Goods> GoodsListReturner(string query)
        {
            var list = new List<Goods>();

            SqlDataReader reader = UseQuery(query).ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Goods(Int32.Parse(reader[0].ToString()),
                    reader[1].ToString(),
                    reader[2].ToString(),
                    reader[3].ToString(),
                    reader[4].ToString(),
                    float.Parse(reader[5].ToString()),
                    Int32.Parse(reader[6].ToString()),
                    reader[7].ToString())
                    );
            }
            reader.Close();

            return list;
        }


        private List<string[]> ListReturner(string query)
        {
            var list = new List<string[]>();

            SqlDataReader reader = UseQuery(query).ExecuteReader();
            while (reader.Read())
            {
                var employee = new string[reader.FieldCount];
                for (int i = 0; i < employee.Length; i++)
                {
                    employee[i] = reader[i].ToString();
                }

                list.Add(employee);
            }
            reader.Close();

            return list;
        }


    }
}
