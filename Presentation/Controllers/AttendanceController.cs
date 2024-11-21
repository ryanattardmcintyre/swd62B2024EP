using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class AttendanceController : Controller
    {
        private StudentRepository _studentRepository;
        private SubjectRepository _subjectRepository;
        private GroupRepository _groupRepository;
        private AttendanceRepository _attendanceRepository;

        public AttendanceController(StudentRepository studentRepository, SubjectRepository subjectRepository,
            GroupRepository groupRepository, AttendanceRepository attendanceRepository) {
            _studentRepository= studentRepository;
            _subjectRepository= subjectRepository;
            _groupRepository= groupRepository;
            _attendanceRepository= attendanceRepository;

        }

        public IActionResult Index()
        {
            var groups = _groupRepository.GetGroups();
            var subjects = _subjectRepository.GetSubjects();

            SelectGroupsSubjectsViewModel myModel = new SelectGroupsSubjectsViewModel()
            {
                Subjects = subjects.ToList(),
                Groups = groups.ToList(),
                PastAttendances = new List<DateTime>()
            };

            return View(myModel);
        }

        [HttpPost]
        public IActionResult Index(string groupCode, string subjectCode)
        {

            var groups = _groupRepository.GetGroups();
            var subjects = _subjectRepository.GetSubjects();
            List<DateTime> pastAttendances = _attendanceRepository.GetAttendances(subjectCode, groupCode)
     .GroupBy(x => new DateTime(x.Timestamp.Year, x.Timestamp.Month, x.Timestamp.Day, x.Timestamp.Hour, 0, 0))
     .Select(x => x.Key)
     .ToList();

            SelectGroupsSubjectsViewModel myModel = new SelectGroupsSubjectsViewModel()
            {
                Subjects = subjects.ToList(),
                Groups = groups.ToList(),
                PastAttendances = pastAttendances
            };
            return View(myModel);
        }




        [HttpGet] //this one should be called to load the page where i will be taking note of who is absent/present
        public IActionResult Create(string groupCode, string subjectCode) {

            var list = _studentRepository.GetStudents().Where(x => x.GroupFK == groupCode);

            //i need to pass to the page
            //1) a list of students within the group specified
            //2) the subject code + perhaps details

            AttendanceViewModel myModel = new AttendanceViewModel();
            myModel.Students = list.ToList();
            myModel.Subject = _subjectRepository.GetSubject(subjectCode);
            myModel.Group = groupCode;

            return View(myModel); 
        }

        [HttpPost]
        public IActionResult Create(List<Attendance> attendances) {

            _attendanceRepository.AddAttendances(attendances);
            TempData["message"] = "All attendances saved";
            return RedirectToAction("Index");
        
        }
    }
}
