using Domain.Models;

namespace Presentation.Models
{
    public class StudentWriteViewModel
    {
        public StudentWriteViewModel() {
        Student = new Student();
        }

        public List<Group> Groups { get; set; }

        public Student Student { get; set; }
    }
}
