using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M19G2.IBLL;
using M19G2.DAL;
using M19G2.Models;

using Log = M19G2.Common.Logging.CustomLogger;

namespace M19G2.BLL
{
    public class DeliveryAutomation : IDeliveryAutomation
    {
        private readonly Dictionary<int, TaxiStaff> workforce;
        private readonly UnitOfWork unitOfWork;

        private readonly Queue<int> PendingOrders;

        public DeliveryAutomation()
        {
            this.unitOfWork = new UnitOfWork();
            this.workforce = new Dictionary<int, TaxiStaff>();
            this.PendingOrders = new Queue<int>();

            var col = unitOfWork.AspNetRolesRepository.Get(x => x.Name == "Delivery Service Staff").FirstOrDefault();
            if(col != null)
            {
                var iter = col.AspNetUsers.GetEnumerator();
                while (iter.MoveNext())
                {
                    //merr userat qe jane aktiv
                    if(iter.Current.LockoutEndDateUtc == null)
                    {
                        workforce.Add(iter.Current.Id, new TaxiStaff()
                        {
                            ID = iter.Current.Id,
                            Orders = new List<int>(),
                            WorkInProgress = 0
                        });
                    }

                }
            }
            var orders = unitOfWork.OrdersRepository.Get(x => x.IsCompleted && !x.IsDelivered && !x.IsCancelled).Select(o => o.ID).ToList();
            orders.ForEach(order => AppendShipment(order));  // meeh
        }

        public ICollection<int> GetOrdersForStaff(int taxiId)
        {
            if (workforce.ContainsKey(taxiId))
                return workforce[taxiId].Orders;
            else
                return new List<int>();
        }

        public void AppendShipment(int orderId)
        {
            var freeStaff = (from i in workforce where i.Value.WorkInProgress < 3 select i.Value).FirstOrDefault();
            if (freeStaff != null)
            {
                workforce[freeStaff.ID].WorkInProgress++;
                workforce[freeStaff.ID].Orders.Add(orderId);
                Log.LogInfo($"Assigned order {orderId} to {freeStaff.ID} for delivery.");
            }
            else
            {
                PendingOrders.Enqueue(orderId);
                Log.LogInfo($"No taxi staff available, order {orderId} is pending for delivery.");
            }
        }

        public void CompleteShipment(int orderId, int taxiId)
        {
            if(workforce[taxiId].Orders.Contains(orderId))
            {
                workforce[taxiId].Orders.Remove(orderId);
                workforce[taxiId].WorkInProgress--;
                Log.LogInfo($"Order {orderId} has been delivered by {taxiId}.");
                if (PendingOrders.Count > 0)
                    AppendShipment(PendingOrders.Dequeue());
            }
        }

        public void AddWorkforce(int taxiId)
        {
            var user = unitOfWork.AspNetUsersRepository.GetByID(taxiId);
            if(user.LockoutEndDateUtc == null)
            {
                workforce.Add(taxiId, new TaxiStaff()
                {
                    ID = taxiId,
                    Orders = new List<int>(),
                    WorkInProgress = 0
                });
                var orders = unitOfWork.OrdersRepository.Get(x => x.IsCompleted && !x.IsDelivered && !x.IsCancelled).Select(o => o.ID).ToList();
                orders.ForEach(order => AppendShipment(order));  // meeh
            }
        }
    }
}
