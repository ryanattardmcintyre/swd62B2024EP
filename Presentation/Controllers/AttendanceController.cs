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

        public AttendanceController(StudentRepository studentRepository, SubjectRepository subjectRepository,
            GroupRepository groupRepository) {
            _studentRepository= studentRepository;
            _subjectRepository= subjectRepository;
            _groupRepository= groupRepository;
        }

        public IActionResult Index()
        {
            var groups = _groupRepository.GetGroups();
            var subjects = _subjectRepository.GetSubjects();

            SelectGroupsSubjectsViewModel myModel = new SelectGroupsSubjectsViewModel()
            {
                Subjects = subjects.ToList(),
                Groups = groups.ToList()
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
        public IActionResult Create(List<string> StudentFK, string SubjectFK, List<bool> IsPresent) {
        
            //to do

            return View(); 
        
        }
    }
}
