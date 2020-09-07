using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M19G2.IBLL;
using M19G2.DAL;
using M19G2.Models;

namespace M19G2.BLL
{
    public class StatisticsService : IStatisticsService
    {
        private readonly UnitOfWork unitOfWork;

        private class DishScore
        {
            public int DishID { get; set; }
            public int Score { get; set; }
        }

        public StatisticsService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ICollection<int> GetMostRequestedDishes(int count)
        {
            var scores = new Dictionary<int, DishScore>();

            var dishes = unitOfWork.DishesRepository.Get();
            
            var iter = dishes.GetEnumerator();
            while(iter.MoveNext())
            {
                var dish = iter.Current;
                dish.Orders.ToList().ForEach(x =>
                {
                    if (scores.ContainsKey(dish.ID))
                        scores[dish.ID].Score++;
                    else
                    {
                        scores.Add(dish.ID, new DishScore() { DishID = dish.ID, Score = 1 });
                    }
                });
            }
            if (scores.Count > 0)
            {
                if (count > 0)
                {
                    return scores.Values.OrderByDescending(x => x.Score).Take(count).Select(score => score.DishID).ToList();
                }
                else
                {
                    return scores.Values.OrderByDescending(x => x.Score).Select(score => score.DishID).ToList();
                }
            }
            else
            {
                return new List<int>();
            }
        }

        public ICollection<int> GetMostRequestedDishesByType(int count, int typeId)
        {
            var scores = new Dictionary<int, DishScore>();

            var orders = unitOfWork.OrdersRepository.Get();
            var iter = orders.GetEnumerator();
            while (iter.MoveNext())
            {
                var order = iter.Current;
                order.Dishes.ToList().ForEach(x =>
                {
                    if(x.TypeID == typeId)
                    {
                        if (scores.ContainsKey(x.ID))
                            scores[x.ID].Score++;
                        else
                        {
                            scores.Add(x.ID, new DishScore() { DishID = x.ID, Score = 1 });
                        }
                    }
                });
            }
            if (count > 0)
                return scores.Values.OrderByDescending(x => x.Score).Take(count).Select(score => score.Score).ToList();
            else
                return scores.Values.OrderByDescending(x => x.Score).Select(score => score.Score).ToList();
        }
    }
}
