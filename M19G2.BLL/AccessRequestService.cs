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
    public class AccessRequestService : IAccessRequestService
    {
        private readonly UnitOfWork _unitOfWork;
        public AccessRequestService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<UsersAccessRequest> GetAllRequests()
        {
            return _unitOfWork.UserAccessRequestsRepository.Get(req => req.AccessRequestStatusId == 1).ToList();
        }

        public void CreateAcessRequest(UsersAccessRequest accessRequest)
        {
            accessRequest.AccessRequestStatusId = 1;
            var result = _unitOfWork.UserAccessRequestsRepository.Insert(accessRequest);
            _unitOfWork.Save();
        }

        public UsersAccessRequest GetByEmail(string email)
        {
            return _unitOfWork.UserAccessRequestsRepository.Get(usr => usr.Email == email).FirstOrDefault();
        }

        public UsersAccessRequest GetPendingRequestByEmail(string email)
        {
            return _unitOfWork.UserAccessRequestsRepository.Get(usr => usr.Email == email && usr.AccessRequestStatusId == 1).FirstOrDefault();
        }

        public UsersAccessRequest GetById(int requestId)
        {
            return _unitOfWork.UserAccessRequestsRepository.Get(usr => usr.Id == requestId && usr.AccessRequestStatusId == 1).FirstOrDefault();
        }

        public void AcceptRequest(int requestId)
        {
            var userRequest = _unitOfWork.UserAccessRequestsRepository.GetByID(requestId);
            userRequest.AccessRequestStatusId = 2;
            _unitOfWork.Save();
        }

        public void DenyRequest(int requestId)
        {
            var userRequest = _unitOfWork.UserAccessRequestsRepository.GetByID(requestId);
            userRequest.AccessRequestStatusId = 3;
            _unitOfWork.Save();
        }
    }
}
