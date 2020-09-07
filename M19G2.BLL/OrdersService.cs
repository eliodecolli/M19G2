using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using M19G2.IBLL;
using M19G2.Models;
using M19G2.DAL;
using M19G2.Common.Logging;
using M19G2.DAL.Entities;

namespace M19G2.BLL
{
    public class OrdersService : IOrdersService
    {

        #region Convertions
        private OrderDto FromEF(Order ef)
        {
            if (ef == null)
                return null;
            var order = new OrderDto()
            {
                AddressID = ef.AddressID,
                UserID = ef.UserID,
                OrderID = ef.ID,
                ETA = ef.ETA,
                IsDelivered = ef.IsDelivered,
                Message = ef.Message,
                Created = ef.DateCreated,
                Active = !ef.IsCancelled
            };
            if(ef.Dishes.Count > 0)
            {
                ef.Dishes.ToList().ForEach(x => {
                    var ocp = unitOfWork.OrderQuantitiesRepository.GetByIDs(new object[] {  ef.ID, x.ID });
                    List<int> aye = new List<int>();
                    if (ocp != null)
                    {
                        for (int i = 0; i < ocp.Quantity; i++)
                            aye.Add(ocp.DishID);
                    }
                    else
                    {
                        aye.Add(x.ID);
                    }
                    aye.ForEach(p => order.DishesIDs.Add(p));
                });

            }
            return order;
        }

        private Order NewFromDTO(OrderDto dto)
        {
            var order = new Order()
            {
                AddressID = dto.AddressID,
                UserID = dto.UserID,
                Dishes = new List<Dish>(),
                Message = dto.Message,
                ETA = dto.ETA,
                IsCancelled = false,
                IsCompleted = false,
                IsDelivered = false
            };
            dto.DishesIDs.ForEach(x => order.Dishes.Add(unitOfWork.DishesRepository.GetByID(x)));
            order.AspNetUser = unitOfWork.AspNetUsersRepository.GetByID(order.UserID); 

            return order;
        }
        #endregion


        private readonly UnitOfWork unitOfWork;

        public OrdersService(UnitOfWork unitOfWork) =>
            this.unitOfWork = unitOfWork;

        public void CancelOrder(int orderId)
        {
            var order = unitOfWork.OrdersRepository.GetByID(orderId);
            if (order != null)
            {
                order.IsCancelled = true;
                unitOfWork.Save();
                CustomLogger.LogInfo($"Order {orderId} has been canceled.");
            }
        }

        public int CreateNewOrder(OrderDto order)
        {
            var created = NewFromDTO(order);
            created.DateCreated = DateTime.Now;
            created = unitOfWork.OrdersRepository.Insert(created);
            unitOfWork.Save();

            Dictionary<int, int> quantity = new Dictionary<int, int>();

            foreach(var v in created.Dishes)
            {
                if (quantity.ContainsKey(v.ID))
                    quantity[v.ID]++;
                else
                    quantity.Add(v.ID, 1);
            }

            foreach (var vk in quantity)
                unitOfWork.OrderQuantitiesRepository.Insert(new OrderQuantity() { DishID = vk.Key, OrderID = created.ID, Quantity = vk.Value });

            
            unitOfWork.Save();
            CustomLogger.LogInfo($"Created new order for user {created.AspNetUser.UserName} with ID {created.ID}");
            return created.ID;
        }

        public OrderDto GetOrderById(int orderId)
        {
            var retval = unitOfWork.OrdersRepository.GetByID(orderId);
            return FromEF(retval);
        }

        public ICollection<OrderDto> ListCompletedOrders()
        {
            var retval = unitOfWork.OrdersRepository.Get(x => x.IsCompleted);

            return retval.Select(x => FromEF(x)).ToList();
        }

        public ICollection<OrderDto> ListOrdersForAddress(int addressId)
        {
            var retval = unitOfWork.OrdersRepository.Get(x => x.AddressID == addressId);

            return retval.Select(x => FromEF(x)).ToList();
        }

        public ICollection<OrderDto> ListOrdersForUser(int userId)
        {
            var retval = unitOfWork.OrdersRepository.Get(x => x.UserID == userId);

            return retval.Select(x => FromEF(x)).ToList();
        }

        public void MarkOrderAsCompleted(int orderId, bool completed)
        {
            var order = unitOfWork.OrdersRepository.GetByID(orderId);
            if (order != null)
            {
                order.IsCompleted = true;
                unitOfWork.Save();

                CustomLogger.LogInfo($"Order {orderId} has been marked as completed.");
            }
        }
    }
}
