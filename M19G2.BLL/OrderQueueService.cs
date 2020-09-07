using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using M19G2.Models;
using M19G2.IBLL;
using M19G2.DAL;
using Log = M19G2.Common.Logging.CustomLogger;

namespace M19G2.BLL
{
    public class OrderQueueService : IOrderQueueService
    {
        private readonly UnitOfWork unitOfWork;
        private readonly List<KitchenStaff> Workforce;

        private readonly Queue<int> PendingOrders;

        public OrderQueueService()
        {
            this.unitOfWork = new UnitOfWork();
            PendingOrders = new Queue<int>();

            var staff = unitOfWork.AspNetRolesRepository.GetByID(3).AspNetUsers.Where(user => user.LockoutEndDateUtc == null);  // only active users
            if (staff != null)
            {
                Workforce = staff.Select(user => new KitchenStaff()
                {
                    ID = user.Id,
                    Name = user.UserName,
                    Capacity = user.OrdersCapacity,
                    Workload = new Queue<int>()
                }).ToList();
            }
            else
                Workforce = new List<KitchenStaff>();

            // load pending orders from the database (this might take some time if we have lots of uncompleted orders)
            var iter = unitOfWork.OrdersRepository.Get(x => !x.IsCompleted).GetEnumerator();
            while (iter.MoveNext())
                PushOrder(iter.Current.ID);
        }

        public void AddWorkforce(KitchenStaff staff)
        {
            Workforce.Add(staff);
            if (PendingOrders.Count > 0)
            {
                lock (PendingOrders)
                {
                    PushOrder(PendingOrders.Dequeue());
                }
            }
        }

        public List<int> GetOrdersInQueue(int staffId)
        {
            var retval = new List<int>();
            var bb = (from i in Workforce where i.ID == staffId select i).FirstOrDefault();
            if(bb != null)
            {
                var iter = bb.Workload.GetEnumerator();
                while (iter.MoveNext())
                    retval.Add(iter.Current);
            }
            return retval;
        }

        public int PeekCurrentOrder(int staffId)
        {
            var a = (from i in Workforce where i.ID == staffId select i).First().Workload;
            if (a.Count > 0)
                return a.Peek();
            return -1;
        }

        public int PopOrder(int staffId)
        {
            var user = (from i in Workforce where i.ID == staffId select i).FirstOrDefault();
            if(user != null)
            {
                var id = user.Workload.Dequeue();
                Log.LogInfo($"Staff {staffId} has completed order {id}");
                if (PendingOrders.Count > 0)
                    PushOrder(PendingOrders.Dequeue());
                return id;
            }
            return 0;
        }

        public string OrderStatus(int orderId)
        {
            var iter = PendingOrders.GetEnumerator();
            var order = unitOfWork.OrdersRepository.GetByID(orderId);
            foreach (var d in order.Dishes)
            {
                if (!d.IsActive)
                {
                    return $"{d.Name} is not available anymore, your order cannot be completed.";
                }
            }
            while (iter.MoveNext())
                if (iter.Current == orderId)
                    return "Our kitchen staff seems to be busy right now, your order is now pending";
            return "";
        }

        public void PushOrder(int orderId)
        {
            //(from i in Workforce where i.Capacity > i.Workload.Count select i).First().Workload.Enqueue(orderId);
            if (!unitOfWork.OrdersRepository.GetByID(orderId).IsCancelled)
            {
                for (int i = 0; i < Workforce.Count; i++)
                {
                    if (Workforce[i].Capacity > Workforce[i].Workload.Count)
                    {
                        Workforce[i].Workload.Enqueue(orderId);
                        Log.LogInfo($"Order {orderId} has been assigned to {Workforce[i].Name} ({Workforce[i].ID})");
                        return;
                    }
                }
                PendingOrders.Enqueue(orderId);
                Log.LogInfo($"There was no staff available, so order {orderId} is pending for assignment");
            }
            else
                Log.LogError($"Cannot enqueue order {orderId} because it's canceled.");
        }
    }
}