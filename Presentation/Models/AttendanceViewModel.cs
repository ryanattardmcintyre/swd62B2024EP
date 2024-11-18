using Domain.Models;

namespace Presentation.Models
{
    public class AttendanceViewModel
    {
        public Subject Subject { get; set; }
        public List<Student> Students { get; set; }

        public string Group { get; set; }
    }
}
