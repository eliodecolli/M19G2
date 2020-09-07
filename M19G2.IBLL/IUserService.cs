using System.Collections.Generic;
using M19G2.DAL.Entities;
using M19G2.Models;

namespace M19G2.IBLL
{
    public interface IUserService
    {
        List<UserDto> GetAllActiveUsers();
        List<UserDto> GetByEmail(string email);
        void AnonimyzeUser(int userId);
        void AssignUserToRole(int userid, string role);

        ICollection<UserAddressDto> GetUserAddresses(int id);

        void AddUserAddress(int userId, string streetName);

        UserAddressDto GetUserAddress(int id);
        AspNetUser GetUserById(int id);
        void AddUser(AspNetUser user);
        void UpdateUser(AspNetUser user);
        List<UserDto> GetAllInactiveUsers();
    }
}
