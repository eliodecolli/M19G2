using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using M19G2.Models;

namespace M19G2.SessionStorage
{
    public interface ISessionStorage
    {
        StorageOrder GetCurrentOrder();

        void ResetOrder(int userid);

        /// <summary>
        /// Checks whether the current order is NULL or not.
        /// </summary>
        /// <returns></returns>
        bool IsOrderReady();

        void SetOrder(StorageOrder value);
    }
}
