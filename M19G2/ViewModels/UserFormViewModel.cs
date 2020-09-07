using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using M19G2.DAL.Entities;

namespace M19G2.ViewModels
{
    public class UserFormViewModel
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [StringLength(1)]
        public string Gender { get; set; }
        public DateTime? Birthday { get; set; }

        [Display(Name = "Role")]
        [Required]
        public string AspNetRoleName { get; set; }

        [Required]
        [StringLength(256)]
        [Phone]
        public string PhoneNumber { get; set; }
        public List<AspNetRole> AspNetRoles { get; set; }
    }
}