using Domain.Models;

namespace Presentation.Models
{
    public class SelectGroupsSubjectsViewModel
    {
        public List<Group> Groups { get; set; }
        public List<Subject> Subjects { get; set; }


        public List<AttendanceForAGroupViewModel> PastAttendances { get; set; }
    }

    public class AttendanceForAGroupViewModel
    {
        public string GroupCode { get; set; }
        public string SubjectCode { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
