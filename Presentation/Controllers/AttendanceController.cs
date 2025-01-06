using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class AttendanceController : Controller
    {
        private IStudentRepository _studentRepository;
        private SubjectRepository _subjectRepository;
        private GroupRepository _groupRepository;
        private AttendanceRepository _attendanceRepository;

        public AttendanceController(IStudentRepository studentRepository, SubjectRepository subjectRepository,
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
            
            //forms a list of grouped (by subject + class + time) attendances
             var pastAttendances = _attendanceRepository.GetAttendances().GroupBy(x => new
            {
                SubjectCode = x.SubjectFK,
                GroupCode = x.Student.GroupFK,
                Timestamp = x.Timestamp
            }).Select(x => new AttendanceForAGroupViewModel()
            {
                 SubjectCode = x.Key.SubjectCode,
                  GroupCode = x.Key.GroupCode,
                   Timestamp = x.Key.Timestamp
            })
            .ToList(); // .ToList() it runs the code...so you re able to inspect the variable with the data



            SelectGroupsSubjectsViewModel myModel = new SelectGroupsSubjectsViewModel()
            {
                Subjects = subjects.ToList(),
                Groups = groups.ToList(),
                PastAttendances = pastAttendances //this will be now filled with the data obtained from the db
            };

            return View(myModel);
        }


        [HttpGet] //this one should be called to load the page where i will be taking note of who is absent/present
        public IActionResult Create(string groupCode, string subjectCode, string whichButton) {


            if (whichButton == "0") //user has decided to create a new attendance
            {
                      var list = _studentRepository.GetStudents().Where(x => x.GroupFK == groupCode);

                                //i need to pass to the page
                                //1) a list of students within the group specified
                                //2) the subject code + perhaps details

                      AttendanceViewModel myModel = new AttendanceViewModel();
                      myModel.Students = list.ToList();
                      myModel.Subject = _subjectRepository.GetSubject(subjectCode);
                      myModel.Group = groupCode;

                      ViewBag.update = false; //this is an alternative way how to pass data to the View not using the myModel

                      return View(myModel); 
            }
            else
            { //user has decided to view an old attendance
                string[] myValues = whichButton.Split(new char[] { '|' });
                string selectedSubjectCode = myValues[1];
                string selectedGroupCode = myValues[0];
                DateTime selectedTimestamp = Convert.ToDateTime(myValues[2]);

                var list = _studentRepository.GetStudents().Where(x => x.GroupFK == selectedGroupCode);
              
                AttendanceViewModel myModel = new AttendanceViewModel();
                myModel.Students = list.ToList();
                myModel.Subject = _subjectRepository.GetSubject(selectedSubjectCode);
                myModel.Group = selectedGroupCode;

                myModel.Presences = _attendanceRepository.GetAttendances().
                    Where(x => x.SubjectFK == selectedSubjectCode && x.Student.GroupFK == selectedGroupCode
                    && x.Timestamp.Year == selectedTimestamp.Year
                    && x.Timestamp.Month == selectedTimestamp.Month
                    && x.Timestamp.Day == selectedTimestamp.Day
                    && x.Timestamp.Hour == selectedTimestamp.Hour
                    && x.Timestamp.Minute == selectedTimestamp.Minute).Select(x => new PresencesViewModel()
                    {Id= x.Id, Present = x.IsPresent }).ToList();

                ViewBag.update = true;

                return View(myModel);

            }

          
        }

        [HttpPost]
        public IActionResult Create(List<Attendance> attendances, string subjectCode, string groupCode,bool update=false) {

            if (update)
            {

                _attendanceRepository.UpdateAttendances(attendances, subjectCode, groupCode);
            }
            else
            {
                _attendanceRepository.AddAttendances(attendances);
            }

            TempData["message"] = "All attendances saved";

            return RedirectToAction("Index");
        
        }




 

    }
}
