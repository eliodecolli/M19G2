using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M19G2.DAL.Entities;

namespace M19G2.IBLL
{
    public interface IRolesService
    {
        List<AspNetRole> GetAllRoles();
        List<AspNetRole> GetRolesForAccessRequest();
        AspNetRole GetRoleOfUser(int id);
    }
}
