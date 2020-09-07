using M19G2.DAL;
using M19G2.DAL.Entities;
using M19G2.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M19G2.BLL
{
    public class LogService : ILogService
    {
        private readonly UnitOfWork _internalUnitOfWork;
        public LogService(UnitOfWork unitOfWork)
        {
            _internalUnitOfWork = unitOfWork;
        }

        public List<Log4NetLog> GetAllLogs()
        {
            return _internalUnitOfWork.Log4NetRepository.Get().ToList();
        }
    }
}
