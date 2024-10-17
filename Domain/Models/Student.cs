using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Student
    {
        [Key()] //specifies the primary key
        public string IdCard { get; set; }
        public string Name { get; set; }    
        public string LastName { get; set; }

        //foreign key property
        public string GroupFK { get; set; }

        //navigational property
        [ForeignKey("GroupFK")] 
        public virtual Group Group { get; set; }



    }
}
