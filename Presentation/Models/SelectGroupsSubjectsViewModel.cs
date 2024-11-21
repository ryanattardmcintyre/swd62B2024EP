using Domain.Models;

namespace Presentation.Models
{
    public class SelectGroupsSubjectsViewModel
    {
        public List<Group> Groups { get; set; }
        public List<Subject> Subjects { get; set; }


        public List<DateTime> PastAttendances { get; set; }
    }
}
