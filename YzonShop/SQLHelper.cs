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

        #region Login

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

        #endregion Login

        #region Goods
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

        public void UpdateGoods(Goods goods)
        {
            string query = $"update Goods set name = '{goods.Name}', firm = '{goods.Firm}', model = '{goods.Model}', description = '{goods.Description}', cost = {goods.Cost.ToString().Replace(',', '.')}, warranty = {goods.Warranty}, image = '{goods.Image}' where id = {goods.Id}";
            UseQuery(query).ExecuteNonQuery();
        }

        public void DeleteGoods(int id)
        {
            string query = $"Delete Goods where id = {id}";
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
        #endregion Goods

        #region Shops
        public List<Shop> GetShops()
        {
            string query = $"select * from Shops";

            return ShopsListReturner(query);

        }

        public void SetShop(Shop shop)
        {
            string query = $"insert into Shops values ('{shop.EmailAddress}', '{shop.DeliverPay}')";
            UseQuery(query).ExecuteNonQuery();
        }

        public void UpdateShop(Shop shop)
        {
            string query = $"update Shops set email_address = '{shop.EmailAddress}', deliver_pay = '{shop.DeliverPay}' where id = {shop.Id}";
            UseQuery(query).ExecuteNonQuery();
        }

        public void DeleteShop(int id)
        {
            string query = $"Delete Shops where id = {id}";
            UseQuery(query).ExecuteNonQuery();
        }

        private List<Shop> ShopsListReturner(string query)
        {
            var list = new List<Shop>();

            SqlDataReader reader = UseQuery(query).ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Shop(Int32.Parse(reader[0].ToString()),
                    reader[1].ToString(),
                    Boolean.Parse(reader[2].ToString())
                    ));
            }
            reader.Close();

            return list;
        }

        #endregion Shops
        #region Orders
        public List<Order> GetOrders()
        {
            string query = $"select * from Orders";

            return OrdersListReturner(query);

        }

        public void SetOrder(Order order)
        {
            //string query = $"insert into Shops values ('{order.EmailAddress}', '{order.DeliverPay}')";
            //UseQuery(query).ExecuteNonQuery();
        }

        public void UpdateOrder(Order order)
        {
            //string query = $"update Shops set email_address = '{order.EmailAddress}', deliver_pay = '{order.DeliverPay}' where id = {order.Id}";
            //UseQuery(query).ExecuteNonQuery();
        }

        public void DeleteOrder(int id)
        {
            string query = $"Delete Order where id = {id}";
            UseQuery(query).ExecuteNonQuery();
        }

        private List<Order> OrdersListReturner(string query)
        {
            var list = new List<Order>();

            SqlDataReader reader = UseQuery(query).ExecuteReader();
            while (reader.Read())
            {
                //list.Add(new Shop(Int32.Parse(reader[0].ToString()),
                //    reader[1].ToString(),
                //    Boolean.Parse(reader[2].ToString())
                //    ));
            }
            reader.Close();

            return list;
        }
        #endregion Orders

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
