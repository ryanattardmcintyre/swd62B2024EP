using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Attendance
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("StudentFK")]
        public Student Student{ get; set; }
        public string StudentFk { get; set; }

        public bool IsPresent { get; set; }

        public DateTime Timestamp { get; set; }

        public string SubjectFK { get; set; }
        [ForeignKey("SubjectFK")]
        public Subject Subject { get; set; }
    }
}
