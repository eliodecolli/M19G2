using System;
using System.ComponentModel.DataAnnotations;

namespace M19G2.DAL.Entities
{
    public partial class Log4NetLog
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        [Required]
        [StringLength(255)]
        public string Thread { get; set; }

        [Required]
        [StringLength(50)]
        public string Level { get; set; }

        [Required]
        [StringLength(255)]
        public string Logger { get; set; }

        [Required]
        [StringLength(4000)]
        public string Message { get; set; }

        [StringLength(2000)]
        public string Exception { get; set; }
    }
}
