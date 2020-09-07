using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using M19G2.Models;

namespace M19G2.IBLL
{
    public interface IStatisticsService
    {
        /// <summary>
        /// Merre n-dishes me te porositura.
        /// </summary>
        /// <param name="count">Sa dishes te marre.</param>
        /// <returns></returns>
        ICollection<int> GetMostRequestedDishes(int count);

        ICollection<int> GetMostRequestedDishesByType(int count, int typeId);
    }
}
