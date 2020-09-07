using M19G2.DAL;
using M19G2.DAL.Entities;
using M19G2.IBLL;
using M19G2.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace M19G2.BLL
{
    public class UserService : IUserService
    {
        private readonly UnitOfWork _internalUnitOfWork;

        public UserService(UnitOfWork unitOfWork)
        {
            _internalUnitOfWork = unitOfWork;
        }

        public void AddUser(AspNetUser user)
        {
            _internalUnitOfWork.AspNetUsersRepository.Insert(user);
            _internalUnitOfWork.Save();
        }

        public void AnonimyzeUser(int userId)
        {
            var userInDb = _internalUnitOfWork.AspNetUsersRepository.GetByID(userId);
            userInDb.FirstName = Guid.NewGuid().ToString();
            userInDb.LastName = Guid.NewGuid().ToString();
            userInDb.Email = Guid.NewGuid().ToString();
            userInDb.Gender = null;
            userInDb.UserName = Guid.NewGuid().ToString();
            userInDb.Birthday = null;
            userInDb.PhoneNumber = Guid.NewGuid().ToString();
            userInDb.AspNetRoles.Clear();
            //kontrollo nese nuk eshte disabled beje
            if (userInDb.LockoutEndDateUtc == null)
            {
                userInDb.LockoutEndDateUtc = DateTime.MaxValue;
            }

            _internalUnitOfWork.AspNetUsersRepository.Update(userInDb);
            _internalUnitOfWork.Save();
        }

        public void AddUserAddress(int userId, string streetName)
        {
            UserAddress ua = new UserAddress() { UserID = userId, StreetName = streetName };
            _internalUnitOfWork.UserAddressesRepository.Insert(ua);
            _internalUnitOfWork.Save();
        }

        public void AssignUserToRole(int userid, string role)
        {
            throw new System.NotImplementedException();
        }

        public List<UserDto> GetAllActiveUsers()
        {
            return _internalUnitOfWork.AspNetUsersRepository.Get(x => x.LockoutEndDateUtc == null).Select(
                x => new UserDto
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    Gender = x.Gender,
                    PhoneNumber = x.PhoneNumber,
                    RoleDto = new RoleDto
                    {
                        Id = x.AspNetRoles.First().Id,
                        Name = x.AspNetRoles.First().Name
                    },
                    Birthdate = x.Birthday
                }
                ).ToList();
        }

        public List<UserDto> GetAllInactiveUsers()
        {
            //get only users that are inactive and not deleted
            var users = _internalUnitOfWork.AspNetUsersRepository.Get(x => x.LockoutEndDateUtc != null && x.AspNetRoles.Count() != 0)
                .Select(
                    x => new UserDto
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Email = x.Email,
                        Gender = x.Gender,
                        PhoneNumber = x.PhoneNumber,
                        RoleDto = new RoleDto
                        {
                            Id = x.AspNetRoles.First().Id,
                            Name = x.AspNetRoles.First().Name
                        },
                        Birthdate = x.Birthday
                    }
                ).ToList();
            return users;
        }

        public List<UserDto> GetByEmail(string email)
        {
            return _internalUnitOfWork.AspNetUsersRepository.Get(x => x.Email.Equals(email)).Select(x => new UserDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Gender = x.Gender,
                PhoneNumber = x.PhoneNumber,
                RoleDto = new RoleDto
                {
                    Id = x.AspNetRoles.First().Id,
                    Name = x.AspNetRoles.First().Name
                },
                Birthdate = x.Birthday
            }).ToList();
        }

        public UserAddressDto GetUserAddress(int id)
        {
            var a = _internalUnitOfWork.UserAddressesRepository.GetByID(id);
            return new UserAddressDto()
            {
                ID = a.ID,
                Name = a.StreetName
            };
        }

        public ICollection<UserAddressDto> GetUserAddresses(int id)
        {
            return _internalUnitOfWork.AspNetUsersRepository.GetByID(id).UserAddresses.Select(x =>
            {
                return new UserAddressDto() { ID = x.ID, Name = x.StreetName };
            }).ToList();
        }

        public AspNetUser GetUserById(int id)
        {
            return _internalUnitOfWork.AspNetUsersRepository.GetByID(id);
        }

        public void UpdateUser(AspNetUser user)
        {
            _internalUnitOfWork.AspNetUsersRepository.Update(user);
        }
    }
}
