using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M19G2.DAL;
using M19G2.DAL.Entities;
using M19G2.IBLL;

namespace M19G2.BLL
{
    public class RolesService : IRolesService
    {
        private readonly UnitOfWork _unitOfWork;

        public RolesService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<AspNetRole> GetAllRoles()
        {
           return _unitOfWork.AspNetRolesRepository.Get().ToList();
        }

        public AspNetRole GetRoleOfUser(int id)
        {
            var user = _unitOfWork.AspNetUsersRepository.GetByID(id);
            return user.AspNetRoles.First();
        }

        public List<AspNetRole> GetRolesForAccessRequest()
        {
            var roles = _unitOfWork.AspNetRolesRepository.Get
                (role => role.Name != "Admin" && role.Name != "Client").ToList();
            return roles;
        }
    }
}
