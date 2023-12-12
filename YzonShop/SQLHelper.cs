﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
