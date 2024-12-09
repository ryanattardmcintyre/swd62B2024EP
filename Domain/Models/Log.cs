using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Log
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //makes sure that the Id is auto-increment
        [Key]
        public int Id { get; set; }

        public string Message { get; set; }

        public DateTime Timestamp { get; set; }

        public string User { get; set; }

        public string IpAddress { get; set; }

    }
}
