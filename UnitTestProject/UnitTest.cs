using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLHelperLib;
using System;
using System.Linq;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestEmptyLoginPassLogin()
        {
            try
            {
                var sql = SQLHelper.GetSQLHelper();
                sql.Login("", "");
            }
            catch (Exception e)
            {
                StringAssert.Contains(e.Message, "Поля логина и пароля должны быть заполнены");
                return;
            }
            Assert.Fail("Ожидаемое исключение не получено.");
        }

        [TestMethod]
        public void TestAccessibleLogin()
        {
            var expected = new string[] { "Администратор", "11" };
            var actual = SQLHelper.GetSQLHelper().Login("admin", "admin");

            CollectionAssert.AreEqual(expected, actual, "Ожидаемый результат не получен!");
        }

        [TestMethod]
        public void TestSQLHelperUniqueInstance()
        {
            var expected = SQLHelper.GetSQLHelper();
            var actual = SQLHelper.GetSQLHelper();

            Assert.AreEqual(expected, actual, "Ожидаемый результат не получен!");
        }

        [TestMethod]
        public void TestFindShopByID()
        {
            var sql = SQLHelper.GetSQLHelper();
            var Shops = sql.GetShops();
            var expected = Shops.First().Id;
            var actual = sql.FindShop(Shops, expected);

            Assert.AreEqual(expected, actual.Id, "Ожидаемый результат не получен!");

        }

        [TestMethod]
        public void TestFindGoodsByID()
        {
            var sql = SQLHelper.GetSQLHelper();
            var items = sql.GetGoods();
            var expected = items.First().Id;
            var actual = sql.FindGoods(items, expected);

            Assert.AreEqual(expected, actual.Id, "Ожидаемый результат не получен!");

        }

        [TestMethod]
        public void TestFindOrderByID()
        {
            var sql = SQLHelper.GetSQLHelper();
            var items = sql.GetOrders();
            var expected = items.First().Id;
            var actual = sql.FindOrder(items, expected);

            Assert.AreEqual(expected, actual.Id, "Ожидаемый результат не получен!");

        }

        [TestMethod]
        public void TestFindUserByID()
        {
            var sql = SQLHelper.GetSQLHelper();
            var items = sql.GetUsers();
            var expected = items.First().Id;
            var actual = sql.FindClient(items, expected);

            Assert.AreEqual(expected, actual.Id, "Ожидаемый результат не получен!");

        }


        [TestMethod]
        public void TestApplyOrderMethod()
        {
            var sql = SQLHelper.GetSQLHelper();
            var items = sql.GetOrders();
            var expected = !items.First().Apply;

            sql.ApplyOrder(items.First());
            var actual = sql.GetOrders().First().Apply;

            Assert.AreEqual(expected, actual, "Ожидаемый результат не получен!");

        }

        [TestMethod]
        public void TestGetSortedGoodsMethod()
        {
            var sql = SQLHelper.GetSQLHelper();
            var items = sql.GetGoods();
            var expected = items.First();

            var actual = sql.GetSortedGoods(expected.Name, expected.Model).First();

            Assert.AreEqual(expected.Name, actual.Name, "Ожидаемый результат не получен!");

        }

        [TestMethod]
        public void TestGetSortedGoodsMethodWithEmptyAttributes()
        {
            var sql = SQLHelper.GetSQLHelper();
            var items = sql.GetGoods();
            var expected = items;

            var actual = sql.GetSortedGoods("", "");

            Assert.AreEqual(expected.Count, actual.Count, "Ожидаемый результат не получен!");

        }
    }
}
