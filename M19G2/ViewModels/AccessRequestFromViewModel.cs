using M19G2.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace M19G2.ViewModels
{
    public class AccessRequestFromViewModel
    {
        public UsersAccessRequest UserAccessRequests { get; set; }
        public List<AspNetRole> AspNetRoles { get; set; }
    }
}