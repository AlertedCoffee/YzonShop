using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLHelperLib.Model;

namespace SQLHelperLib
{
    /// <summary>
    /// Класс подключения к базе.
    /// </summary>
    public class SQLHelper
    {
        /// <summary>
        /// Поле подключения.
        /// </summary>
        private SqlConnection Connection;

        /// <summary>
        /// Строка подключения
        /// </summary>
        private string connectionString = @" Data Source=ALERTEDCOFFEE\SQLEXPRESS;Initial Catalog=EShop;Integrated Security=True; ";

        /// <summary>
        /// Поле объекта класса SQLHelper.
        /// </summary>
        private static SQLHelper _SQLHelper;

        /// <summary>
        /// Приватный конструктор.
        /// </summary>
        private SQLHelper()
        {
            Connection = new SqlConnection(connectionString);
            Connection.Open();
        }

        /// <summary>
        /// Возвращает единственный объект SQLHelper.
        /// </summary>
        /// <returns>SQLHelper</returns>
        public static SQLHelper GetSQLHelper()
        {
            if (_SQLHelper == null) _SQLHelper = new SQLHelper();
            return _SQLHelper;
        }

        /// <summary>
        /// Инициализация команды.
        /// </summary>
        /// <param name="Query">Текст запроса</param>
        /// <returns>SqlCommand</returns>
        private SqlCommand UseQuery(string Query)
        {
            return new SqlCommand(Query, Connection);
        }

        #region Login

        /// <summary>
        /// Проверка авторизации.
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <returns>string[]</returns>
        /// <exception cref="Exception">Исключение на не введенные login и password</exception>
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

        /// <summary>
        /// Сохранение данных об авторизации.
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="access">Успешный/не успешный доступ</param>
        public void SaveAuthLog(string login, int access)
        {
            string Query = $"insert into AuthLog values ('{login}', '{DateTime.Now}', {access})";
            UseQuery(Query).ExecuteNonQuery();
        }

        /// <summary>
        /// Получение данных об авторизациях.
        /// </summary>
        /// <returns>List<AuthLog></returns>
        public List<AuthLog> GetAuthList()
        {
            string query = $"select * from AuthLog";

            return AuthListReturner(query);
        }

        /// <summary>
        /// Получение данных об авторизации с параметрами поиска.
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="date">Дата</param>
        /// <returns>List<AuthLog></returns>
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

        /// <summary>
        /// Метода инициализации листа List<AuthLog> из запроса.
        /// </summary>
        /// <param name="query">Текст запроса</param>
        /// <returns>List<AuthLog></returns>
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

        /// <summary>
        /// Получение информации о товарах.
        /// </summary>
        /// <returns>List<Goods></returns>
        public List<Goods> GetGoods()
        {
            string query = $"select * from Goods";

            return GoodsListReturner(query);

        }

        /// <summary>
        /// Создание товара.
        /// </summary>
        /// <param name="goods">Товар</param>
        public void SetGoods(Goods goods)
        {
            string query = $"insert into Goods values ('{goods.Name}', '{goods.Firm}', '{goods.Model}', '{goods.Description}', {goods.Cost.ToString().Replace(',', '.')}, {goods.Warranty}, '{goods.Image}')";
            UseQuery(query).ExecuteNonQuery();
        }

        /// <summary>
        /// Обновление товара
        /// </summary>
        /// <param name="goods">Товар</param>
        public void UpdateGoods(Goods goods)
        {
            string query = $"update Goods set name = '{goods.Name}', firm = '{goods.Firm}', model = '{goods.Model}', description = '{goods.Description}', cost = {goods.Cost.ToString().Replace(',', '.')}, warranty = {goods.Warranty}, image = '{goods.Image}' where id = {goods.Id}";
            UseQuery(query).ExecuteNonQuery();
        }

        /// <summary>
        /// Удаление товара.
        /// </summary>
        /// <param name="id">id</param>
        public void DeleteGoods(int id)
        {
            string query = $"Delete Goods where id = {id}";
            UseQuery(query).ExecuteNonQuery();
        }

        /// <summary>
        /// Получение товаров с параметрами поиска.
        /// </summary>
        /// <param name="name">Наименование</param>
        /// <param name="model">Модель</param>
        /// <returns></returns>
        public List<Goods> GetSortedGoods(string name, string model)
        {
            string query;
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(model))
            {
                return GetGoods();
            }
            else if (string.IsNullOrEmpty(name))
            {
                query = $"select * from Goods where model = '{model}'";
            }
            else if (string.IsNullOrEmpty(model))
            {
                query = $"select * from Goods where name = '{name}'";
            }
            else
            {
                query = $"select * from Goods where name = '{name}' and  model = '{model}'";
            }

            return GoodsListReturner(query);
        }

        /// <summary>
        /// Метода инициализации листа List<Goods> из запроса.
        /// </summary>
        /// <param name="query">Текст запроса</param>
        /// <returns>List<Goods></returns>
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

        /// <summary>
        /// Получение информации о магазинах.
        /// </summary>
        /// <returns>List<Shop></returns>
        public List<Shop> GetShops()
        {
            string query = $"select * from Shops";

            return ShopsListReturner(query);

        }

        /// <summary>
        /// Создание магазина
        /// </summary>
        /// <param name="shop">Магазин</param>
        public void SetShop(Shop shop)
        {
            string query = $"insert into Shops values ('{shop.EmailAddress}', '{shop.DeliverPay}')";
            UseQuery(query).ExecuteNonQuery();
        }

        /// <summary>
        /// Обновление информации о магазине.
        /// </summary>
        /// <param name="shop">Магазин</param>
        public void UpdateShop(Shop shop)
        {
            string query = $"update Shops set email_address = '{shop.EmailAddress}', deliver_pay = '{shop.DeliverPay}' where id = {shop.Id}";
            UseQuery(query).ExecuteNonQuery();
        }

        /// <summary>
        /// Удаление магазина.
        /// </summary>
        /// <param name="id">Id</param>
        public void DeleteShop(int id)
        {
            string query = $"Delete Shops where id = {id}";
            UseQuery(query).ExecuteNonQuery();
        }

        ///Метода инициализации листа List<Shop> из запроса.
        /// </summary>
        /// <param name="query">Текст запроса</param>
        /// <returns>List<Shop></returns>
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
        /// <summary>
        /// Получение информации о заказах.
        /// </summary>
        /// <returns>List<Order></returns>
        public List<Order> GetOrders()
        {
            string query = $"select * from Orders";

            return OrdersListReturner(query);

        }

        /// <summary>
        /// Подтверждение заказа
        /// </summary>
        /// <param name="order">Заказ</param>
        public void ApplyOrder(Order order)
        {
            string query = $"update Orders set apply = '{!order.Apply}' where id = {order.Id}";
            UseQuery(query).ExecuteNonQuery();
        }

        /// <summary>
        /// Добавление заказа
        /// </summary>
        /// <param name="goodsId">id товара</param>
        /// <param name="shopId">id магазина</param>
        /// <param name="count">количество</param>
        /// <param name="clientId">id клиента</param>
        public void AddOrder(int goodsId, int shopId, int count, int clientId)
        {
            string query = $"insert into Orders values ('{goodsId}', '{shopId}', '{DateTime.Now}', '{count}', {clientId}, 0)";
            UseQuery(query).ExecuteNonQuery();
        }

        /// <summary>
        /// Метода инициализации листа List<Order> из запроса.
        /// </summary>
        /// <param name="query">Текст запроса</param>
        /// <returns>List<Order></returns>
        private List<Order> OrdersListReturner(string query)
        {
            var list = new List<Order>();
            var goods = GetGoods();
            var shops = GetShops();
            var users = GetUsers();


            SqlDataReader reader = UseQuery(query).ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Order(Int32.Parse(reader[0].ToString()),
                    FindGoods(goods, Int32.Parse(reader[1].ToString())),
                    FindShop(shops, Int32.Parse(reader[2].ToString())),
                    DateTime.Parse(reader[3].ToString()),
                    Int32.Parse(reader[4].ToString()),
                    FindClient(users, Int32.Parse(reader[5].ToString())),
                    Boolean.Parse(reader[6].ToString()))
                    );
            }
            reader.Close();

            return list;
        }

        /// <summary>
        /// Поиск товара по id.
        /// </summary>
        /// <param name="list">Лист товаров</param>
        /// <param name="id">id поиска</param>
        /// <returns>Goods</returns>
        public Goods FindGoods(List<Goods> list, int id)
        {
            foreach (var item in list)
            {
                if (item.Id == id) { return item; }
            }
            return null;
        }

        /// <summary>
        /// Поиск заказа по id.
        /// </summary>
        /// <param name="list">Лист заказов</param>
        /// <param name="id">id поиска</param>
        /// <returns>Order</returns>
        public Order FindOrder(List<Order> list, int id)
        {
            foreach (var item in list)
            {
                if (item.Id == id) { return item; }
            }
            return null;
        }

        /// <summary>
        /// Получение информации о пользователях.
        /// </summary>
        /// <returns>List<User></returns>
        public List<User> GetUsers()
        {
            var list = new List<User>();
            string query = $"select * from Users";
            SqlDataReader reader = UseQuery(query).ExecuteReader();
            while (reader.Read())
            {
                list.Add(new User(
                    Int32.Parse(reader[0].ToString()),
                    reader[1].ToString(),
                    reader[2].ToString()));
            }
            reader.Close();

            return list;
        }

        /// <summary>
        /// Получение информации о доставках.
        /// </summary>
        /// <param name="id">id доставщика</param>
        /// <returns>List<Deliver></returns>
        public List<Deliver> GetDeliver(int id)
        {
            var list = new List<Deliver>();
            List<Order> orders = GetOrders();


            string query = $"select * from Delivers where deliverer_id = {id}";
            SqlDataReader reader = UseQuery(query).ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Deliver(
                    FindOrder(orders, Int32.Parse(reader[0].ToString())),
                    DateTime.Parse(reader[1].ToString()),
                    reader[2].ToString()));
            }
            reader.Close();

            return list;
        }

        /// <summary>
        /// Поиск пользователя по id.
        /// </summary>
        /// <param name="list">Лист пользователей</param>
        /// <param name="id">id поиска</param>
        /// <returns>User</returns>
        public User FindClient(List<User> list, int id)
        {
            foreach (var item in list)
            {
                if (item.Id == id) { return item; }
            }
            return null;
        }

        /// <summary>
        /// Поиск магазина по id.
        /// </summary>
        /// <param name="list">Лист магазинов</param>
        /// <param name="id">id поиска</param>
        /// <returns>Shop</returns>
        public Shop FindShop(List<Shop> list, int id)
        {
            foreach (var item in list)
            {
                if (item.Id == id) { return item; }
            }
            return null;
        }
        #endregion Orders
    }
}
