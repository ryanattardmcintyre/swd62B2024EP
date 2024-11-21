using Domain.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Presentation.Models
{
    public class AttendanceViewModel
    {
        public Subject Subject { get; set; }
        public List<Student> Students { get; set; }

        public string Group { get; set; }

        //public List<AttendanceRecord> attendances { get; set; }
    }
}
