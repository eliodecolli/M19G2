using M19G2.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M19G2.IBLL
{
    public interface ILogService
    {
        List<Log4NetLog> GetAllLogs();
    }
}
