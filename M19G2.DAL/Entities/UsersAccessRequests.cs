using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M19G2.DAL.Entities
{
    public class UsersAccessRequest
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [StringLength(1)]
        public string Gender { get; set; }
        public DateTime? Birthday { get; set; }

        [Required]
        [StringLength(256)]
        [Phone]
        public string PhoneNumber { get; set; }

        public int AspNetRoleId { get; set; }
        public virtual AspNetRole AspNetRole { get; set; }

        public int AccessRequestStatusId { get; set; }
        public virtual AccessRequestStatus AccessRequestStatus { get; set; }
    }
}
