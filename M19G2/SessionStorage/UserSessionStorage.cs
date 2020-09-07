using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using M19G2.Models;

namespace M19G2.SessionStorage
{
    public class UserSessionStorage : ISessionStorage
    {
        public StorageOrder GetCurrentOrder()
        {
            var session = HttpContext.Current.Session;

            var order = session["CurrentOrder"] as StorageOrder;
            return order;  // edhe nqs esht null nuk prish pune, pasi dmth q nk ka shtuar ende asgje ne Cart, ose ka perfudnuar nje Order.
        }

        public bool IsOrderReady()
        {
            var session = HttpContext.Current.Session;
            return session["CurrentOrder"] != null;
        }

        public void ResetOrder(int userid)
        {
            var session = HttpContext.Current.Session;

            session["CurrentOrder"] = new StorageOrder() { CurrentOrder = new OrderDto() {
                UserID = userid,
                Message = ""
            } };
        }

        public void SetOrder(StorageOrder value)
        {
            var session = HttpContext.Current.Session;

            session["CurrentOrder"] = value;
        }
    }
}