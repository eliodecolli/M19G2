using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using M19G2.Models;
using M19G2.Common.Logging;
using M19G2.IBLL;
using M19G2.DAL;

namespace M19G2.BLL
{
    public class DeliveryService : IDeliveryService
    {
        private readonly UnitOfWork unitOfWork;

        public DeliveryService(UnitOfWork unitOfWork) => this.unitOfWork = unitOfWork;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="deliveredStatus">True if delivered, false otherwise.</param>
        public void MarkAsDelivered(int orderId, bool deliveredStatus)
        {
            var order = unitOfWork.OrdersRepository.GetByID(orderId);
            if(order != null)
            {
                order.IsDelivered = deliveredStatus;
                string status = deliveredStatus ? "delivered" : "under delivery";
                unitOfWork.OrdersRepository.Update(order);
                unitOfWork.Save();
                CustomLogger.LogInfo($"Order {order.ID} has been marked as {status}");
            }
        }

        public void SetEstimatedArivalTime(int orderId, int minutes)
        {
            var order = unitOfWork.OrdersRepository.GetByID(orderId);
            if(order != null && !order.IsCancelled && !order.IsDelivered && order.IsCompleted)
            {
                order.ETA = minutes;
                unitOfWork.Save();
                CustomLogger.LogInfo($"Order {order.ID} 's ETA has been set to {minutes} minutes");
            }
        }
    }
}
