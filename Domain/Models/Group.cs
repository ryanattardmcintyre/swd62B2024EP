using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Group
    {
        [Key] //specifying that the Code property is going to be the primary key in the database
        public string Code { get; set; }

        public string Programme { get; set; }

        public List<Student> Students { get; set; }

    }
}
