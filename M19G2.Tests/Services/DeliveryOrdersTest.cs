using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using M19G2.Models;
using M19G2.BLL;

namespace M19G2.Tests.Services
{
    [TestClass]
    public class DeliveryOrdersTest
    {
        [TestMethod]
        public void CreateOrder()
        {
            var srv = new OrdersService(new DAL.UnitOfWork());
            int id = srv.CreateNewOrder(new OrderDto()
            {
                Message = "Heeeey"
            });
            Assert.AreNotEqual(id, 0);
        }

        [TestMethod]
        public void SetOrderAsCompleted()
        {
            dynamic srv = new DeliveryService(new DAL.UnitOfWork());
            srv.MarkAsDelivered(1, true);


            srv = new OrdersService(new DAL.UnitOfWork());
            var ord = srv.GetOrderById(1);
            Assert.IsTrue(ord.IsDelivered);
        }
    }
}
