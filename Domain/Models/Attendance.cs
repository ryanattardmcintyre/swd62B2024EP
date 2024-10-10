using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Attendance
    {
        public string StudentFk { get; set; }

        public bool IsPresent { get; set; }

        public DateTime Timestamp { get; set; }


    }
}
