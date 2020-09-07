using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M19G2.DAL.Entities;

namespace M19G2.IBLL
{
    public interface IAccessRequestService
    {
        void CreateAcessRequest(UsersAccessRequest accessRequest);
        void AcceptRequest(int requestId);
        void DenyRequest(int requestId);
        List<UsersAccessRequest> GetAllRequests();
        UsersAccessRequest GetByEmail(string email);
        UsersAccessRequest GetPendingRequestByEmail(string email);
        UsersAccessRequest GetById(int requestId);
    }
}
